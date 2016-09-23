using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;
using Coevolution.Models;
using System.Web.Http.Cors;

namespace Coevolution.Controllers
{
    //Enable Cross Origin Resource Sharing for local development
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class ItemsController : ApiController
    {
        private ModelContext db = new ModelContext();

        // GET: api/Items
        /// <summary>
        /// Get a list of all top level Items
        /// </summary>
        /// <param name="showDeleted">
        /// Whether or not deleted items/children should be shown
        /// </param>
        [HttpGet]
        public List<DtoItemReduced> GetItems(bool showDeleted = false)
        {
            
            var dbItems = db.Items.Include("Labels").Where(x => (!x.Deleted || showDeleted) && x.Parent == null).ToList();
            if (dbItems != null)
            {
                return dbItems.Select(item => item.ToDtoReduced()).ToList();
            }
            else
            {
                return new List<DtoItemReduced>();
            }
        }

        // GET: api/Items/5
        /// <summary>
        /// Gets a single Item from its Id
        /// </summary>
        /// <param name="id">
        /// The Id of the Item to be retrieved
        /// </param>
        /// <param name="showDeleted">
        /// Whether or not deleted items/children should be shown
        /// </param>
        [ResponseType(typeof(DtoItem))]
        public IHttpActionResult GetItem(int id, bool showDeleted = false)
        {
            var items = db.Items.Include("Labels").Include("Notes").Where(x => x.Id == id && (!x.Deleted || showDeleted));
            if (items.Count() <= 0) {
                return NotFound();
            }
            Item item = items.First();

            //Return Node DTO if item is a node
            //This includes that nodes children
            if(item is Node)
            {
                Node nodeItem = (Node)item;

                var tempList = nodeItem.Children;

                nodeItem.Children = db.Items.Where(x => x.Parent.Id == item.Id && (!x.Deleted || showDeleted)).ToList();
                return Ok(nodeItem.ToDto());
            }

            var dtoItem = item.ToDto();

            return Ok(dtoItem);
        }

        // PUT: api/Items/5
        /// <summary>
        /// Update an Item with a specified Id
        /// </summary>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutItem(int id, DtoItem dtoItem)
        {
            //Check dto received is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check id matches
            if (id != dtoItem.Id)
            {
                return BadRequest();
            }

            //Get the itemfrom the db
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            //Get the item's parent
            Item potentialParent = null;
            if (dtoItem.Parent != null)
            {
                potentialParent = db.Items.Find(dtoItem.Parent);
                if (potentialParent == null)
                {
                    return BadRequest("Can't find parent with specified Id");
                }
                if (potentialParent is Leaf)
                {
                    throw new System.InvalidOperationException("Parent cannot be Leaf");
                }
            }

            //Convert parent to domain object
            //Update the item's key to match the parent, and value if the item is a leaf
            try
            {
                Item new_item = dtoItem.ToDomainObject((Node)potentialParent);

                item.Key = new_item.Key;
                if (dtoItem is DtoLeaf)
                {
                    if (!(item is Leaf))
                    {
                        throw new System.InvalidOperationException("Item type must match.");
                    }
                    ((Leaf)item).Value = ((Leaf)new_item).Value;
                }
            }
            catch (InvalidDataException exception)
            {
                return BadRequest(exception.Message);
            }

            item.Updated();
            db.SaveChanges();
            return Ok();
        }

        // PUT: api/Items/5?noteContent=NewNote
        /// <summary>
        /// Add a note to an existing Item
        /// </summary>
        [ResponseType(typeof(int))]
        public IHttpActionResult PutItem(int id, String noteContent)
        {
            //Create note from string
            Note note = new Note();
            note.Content = noteContent;

            //Find specified item
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            //Add note to item
            item.Notes.Add(note);
            db.SaveChanges();
            return Ok(note.Id);
        }
        
        /// <summary>
        /// Add a label to an existing Item
        /// </summary>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutItem(int id, int labelId)
        {
            //Find the item with the given id
            Item item = db.Items.Include("Labels").First(u => u.Id == id);
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            //Find the label with the given id
            Label label = db.Labels.Find(labelId);
            if (label == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            //Check if already labelled
            if (item.Labels.Contains(label))
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            //Add the label to the item
            item.Labels.Add(label);
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Items
        /// <summary>
        /// Add a new Item to the database
        /// </summary>
        [ResponseType(typeof(DtoItem))]
        public IHttpActionResult PostItem(DtoItem dtoItem)
        {
            //Validate supplied DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get parent item
            Item potentialParent = null;
            if (dtoItem.Parent != null)
            {
                potentialParent = db.Items.Find(dtoItem.Parent);
                if (potentialParent == null)
                {
                    return BadRequest("Can't find parent with specified Id");
                }
                if (potentialParent is Leaf)
                {
                    throw new System.InvalidOperationException("Parent cannot be Leaf");
                }
            }

            //Create item and assign it to its parent as a child
            Item item;
            try
            {
                item = dtoItem.ToDomainObject((Node)potentialParent);

                if (potentialParent != null)
                {
                    ((Node)potentialParent).Children.Add(item);
                }
            }
            catch (InvalidDataException exception)
            {
                return BadRequest(exception.Message);
            }

            item.Created();
            db.Items.Add(item);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.Id }, item.ToDto());
        }

        // DELETE: api/Items/5
        /// <summary>
        /// Remove an Item with the specified Id from the database
        /// </summary>
        [ResponseType(typeof(Item))]
        public IHttpActionResult DeleteItem(int id)
        {
            //Find item with id
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            //Delete item specified, including children, recursively
            List<Item> nodes_to_delete = new List<Item>();
            nodes_to_delete.Add(item);
            while (nodes_to_delete.Count > 0) {
                Item current_node = nodes_to_delete.Last();
                nodes_to_delete.RemoveAt(nodes_to_delete.Count-1);
                List<Item> children = db.Items.Include("Labels").Include("Notes").Where(x => x.Parent.Id == current_node.Id).ToList();
                if (children != null)
                {
                    nodes_to_delete.AddRange(children);
                }
                current_node.Updated();
                current_node.Deleted = true;
            }

            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Remove an label from specified item
        /// </summary>
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteItem(int id, int labelId)
        {
            //Find item with id
            Item item = db.Items.Include("Labels").First(u => u.Id == id);
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            //Find label with id
            Label label = db.Labels.Find(labelId);
            if (label == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            //Remove label from item
            if (!item.Labels.Contains(label))
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
            item.Labels.Remove(label);

            db.SaveChanges();
            return Ok(item);
        }

        // DELETE: api/Items/5
        /// <summary>
        /// Remove a note with the specified Id from the database
        /// </summary>
        [ResponseType(typeof(Item))]
        public IHttpActionResult DeleteNote(int id, int noteId)
        {
            //Find item with id
            Item item = db.Items.Include("Notes").First(u => u.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            //Find note id
            Note note = db.Notes.Find(noteId);
            if (note == null)
            {
                return NotFound();
            }

            //Remove note from item and delete note
            if (!item.Notes.Contains(note))
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
            item.Notes.Remove(note);
            db.Notes.Remove(note);

            db.SaveChanges();
            return Ok(item);
        }

        // Get: api/Items/Search/Note/{query}
        /// <summary>
        /// Search all notes for query string
        /// Returns an array of ids of the nodes containing the string
        /// </summary>
        /// <param name="query">The string being searched for</param>
        /// 
        [Route("api/Items/Search/Note/{query}")]
        public IHttpActionResult GetSearchNotes(string query)
        {
            query = Regex.Escape(query);
            var notes = db.Notes.Where(x => x.Content.Contains(query)).ToArray();
            var dtos = new List<DtoNote>();
            foreach (var note in notes)
            {
                dtos.Add(note.ToDto());
            }
            return Ok(dtos);
        }

        // Get: api/Items/Search/Note/{label}
        /// <summary>
        /// Search all items for label
        /// Returns an array of ids of the nodes containing the string
        /// </summary>
        /// <param name="label">The id of the label being searched for</param>
        /// 
        [Route("api/Items/Search/Label/{label}")]
        public IHttpActionResult GetSearchLabel(Label label)
        {
            var items = db.Items.Include(m => m.Labels).Where(x => x.Labels.Contains(label)).ToArray();
            var dtos = new List<DtoSearchItem>();
            foreach (var item in items)
            {
                dtos.Add(new DtoSearchItem(item));
            }
            return Ok(dtos);
        }

        // Get: api/Items/Search/Key/{query}
        /// <summary>
        /// Search all items keys for query string
        /// Returns an array of ids of the nodes containing the string
        /// </summary>
        /// <param name="query">The string being searched for</param>
        /// 
        [Route("api/Items/Search/Key/{query}")]
        public IHttpActionResult GetSearchKeys(string query)
        {
            query = Regex.Escape(query);
            var items = db.Items.Where(x => x.Key.Contains(query)).ToArray();
            var dtos = new List<DtoSearchItem>();
            foreach(var item in items)
            {
                dtos.Add(new DtoSearchItem(item));
            }
            return Ok(dtos);
        }

        // Get: api/Items/Search/Value/{query}
        /// <summary>
        /// Search all items values for query string
        /// Returns an array of ids of the nodes containing the string
        /// </summary>
        /// <param name="query">The string being searched for</param>
        /// 
        [Route("api/Items/Search/Value/{query}")]
        public IHttpActionResult GetSearchValues(string query)
        {
            query = Regex.Escape(query);
            var items = db.Items.Where(x => x.Parent != null).ToList();

                //.Where(x => x.Value.Contains(query)).ToArray();
            var dtos = new List<DtoSearchItem>();
            foreach (var item in items)
            {
                if (item is Leaf)
                {
                    var leaf = (Leaf)item;
                    if (leaf.Value.Contains(query))
                    {
                        dtos.Add(new DtoSearchItem(leaf));
                    }
                }
                
            }
            return Ok(dtos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.Id == id) > 0;
        }
    }
}