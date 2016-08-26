using System;
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

namespace Coevolution.Controllers
{
    public class ItemsController : ApiController
    {
        private ModelContext db = new ModelContext();

        // GET: api/Items
        /// <summary>
        /// Get a list of all current Items
        /// </summary>
        public List<DtoItem> GetItems()
        {

            var dbItems = db.Items.Include("Labels").Include("Notes").Where(x => x.Deleted == false).ToList().Select(item => item.ToDto()).ToList();


            return dbItems;
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

            var dtoItem = item.ToDto();

            return Ok(dtoItem);
        }

        // PUT: api/Items/5
        /// <summary>
        /// Update an Item with a specified Id
        /// </summary>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutItem(int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add a new Item to the database
        /// </summary>
        // POST: api/Items
        [ResponseType(typeof(DtoItem))]
        public IHttpActionResult PostLeaf(DtoItem dtoItem)
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
                    throw new NullReferenceException("Can't find parent with specified Id"); // TODO: Should return to client a 4XX error, not a 5XX error
                }
                if (potentialParent is Leaf)
                {
                    throw new System.InvalidOperationException("Parent cannot be Leaf");
                }
            }
            Item item = dtoItem.ToDomainObject((Node)potentialParent);

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

            db.Items.Remove(item);
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