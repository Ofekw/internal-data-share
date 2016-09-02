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
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class ItemsController : ApiController
    {
        private ModelContext db = new ModelContext();

        // GET: api/Items
        /// <summary>
        /// Get a list of all current Items
        /// </summary>
        [HttpGet]
        public List<DtoItemReduced> GetItems()
        {

            var dbItems = db.Items.Include("Labels").Where(x => !x.Deleted && x.Parent == null).ToList();
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
        [ResponseType(typeof(DtoItem))]
        public IHttpActionResult GetItem(int id)
        {
            Item item = db.Items.Include("Labels").Include("Notes").Where(x => x.Id == id).First();
            if (item == null)
            {
                return NotFound();
            }

            if(item is Node)
            {
                Node nodeItem = (Node)item;
                nodeItem.Children = db.Items.Where(x => x.Parent.Id == item.Id).ToList();
            }

            var dtoItem = item.ToDto();

            return Ok(dtoItem);
        }

        /// <summary>
        /// Update an Item with a specified Id
        /// </summary>
        // PUT: api/Items/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutItem(int id, DtoItem dtoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dtoItem.Id)
            {
                return BadRequest();
            }

            Item item = db.Items.Find(id);
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

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

        /// <summary>
        /// Add a note to an existing Item
        /// </summary>
        // PUT: api/Items/5?noteContent=NewNote
        [ResponseType(typeof(int))]
        public IHttpActionResult PutItem(int id, String noteContent)
        {
            Note note = new Note();
            note.Content = noteContent;

            Item item = db.Items.Find(id);
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
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
            Item item = db.Items.Include("Labels").First(u => u.Id == id);
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            Label label = db.Labels.Find(labelId);
            if (label == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            if (item.Labels.Contains(label))
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
            item.Labels.Add(label);

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Add a new Item to the database
        /// </summary>
        // POST: api/Items
        [ResponseType(typeof(DtoItem))]
        public IHttpActionResult PostItem(DtoItem dtoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

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
            Item item = db.Items.Include("Labels").First(u => u.Id == id);
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            Label label = db.Labels.Find(labelId);
            if (label == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

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
            Item item = db.Items.Include("Notes").First(u => u.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            Note note = db.Notes.Find(noteId);
            if (note == null)
            {
                return NotFound();
            }
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
        [Route("Items/Search/Note/{query}")]
        [ResponseType(typeof(int[]))]
        public IHttpActionResult GetSearchNotes(string query)
        {
            query = Regex.Escape(query);
            return Ok(db.Notes.Where(x => x.Content.Contains(query)).Select(x => x.Item.ToDto()).ToArray());
        }

        // Get: api/Items/Search/Note/{query}
        /// <summary>
        /// Search all notes for query string
        /// Returns an array of ids of the nodes containing the string
        /// </summary>
        /// <param name="label">The id of the label being searched for</param>
        /// 
        [Route("Items/Search/Note/{label}")]
        [ResponseType(typeof(int[]))]
        public IHttpActionResult GetSearchLabel(Label label)
        {
            return Ok(db.Items.Where(x => x.Labels.Contains(label)).Select(x => x.ToDto()).ToArray());
        }

        // Get: api/Items/Search/Key/{query}
        /// <summary>
        /// Search all items keys for query string
        /// Returns an array of ids of the nodes containing the string
        /// </summary>
        /// <param name="query">The string being searched for</param>
        /// 
        [Route("Items/Search/Key/{query}")]
        [ResponseType(typeof(int[]))]
        public IHttpActionResult GetSearchKeys(string query)
        {
            query = Regex.Escape(query);
            return Ok(db.Items.Where(x => x.Key.Contains(query)).Select(x => x.ToDto()).ToArray());
        }

        // Get: api/Items/Search/Value/{query}
        /// <summary>
        /// Search all items values for query string
        /// Returns an array of ids of the nodes containing the string
        /// </summary>
        /// <param name="query">The string being searched for</param>
        /// 
        [Route("Items/Search/Value/{query}")]
        [ResponseType(typeof(int[]))]
        public IHttpActionResult GetSearchValues(string query)
        {
            query = Regex.Escape(query);
            return Ok(db.Items.Where(x => x.GetType() == typeof(Leaf)).Select(x => (Leaf)x).Where(x => x.Value.Contains(query)).Select(x => x.ToDto()).ToArray());
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