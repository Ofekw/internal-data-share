using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        /// Add a label to an existing Item
        /// </summary>
        // POST: api/Items
        [ResponseType(typeof(void))]
        public IHttpActionResult PutItem(int id, int labelId)
        {
            Item item = db.Items.Include("Labels").First(u => u.Id == id);
            if(item == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            Label label = db.Labels.Find(labelId);
            if (label == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            if(item.Labels.Contains(label))
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