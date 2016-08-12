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
            var dbItems = db.Items.Include("Labels").Include("Notes").Where(x => x.Deleted == false).ToList().Select(item => DtoConverter.ConvertToDto(item)).ToList();

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

            var dtoItem = DtoConverter.ConvertToDto(item);

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

        // POST: api/Items
        /// <summary>
        /// Add a new Item to the database
        /// </summary>
        [ResponseType(typeof(Item))]
        public IHttpActionResult PostItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Items.Add(item);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.Id }, item);
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