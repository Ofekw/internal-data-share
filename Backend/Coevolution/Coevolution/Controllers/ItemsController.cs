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
        public IQueryable<DtoItem> GetItems()
        {
            var dtoItems = from d in db.Items
                            where !d.Deleted
                            select new DtoItem()
                            {
                                Id = d.Id,
                                Key = d.Key,
                                Parent = d.Parent.Id,
                                Date = d.Date,
                                Deleted = d.Deleted,
                                Labels = d.Labels,
                                Notes = d.Notes
                            };
            return dtoItems;
        }

        // GET: api/Items/5
        [ResponseType(typeof(DtoItem))]
        public IHttpActionResult GetItem(int id)
        {
            Item item = db.Items.Find(id);
            if (item == null || item.Deleted)
            {
                return NotFound();
            }

            return Ok(new DtoItem()
                            {
                                Id = item.Id,
                                Key = item.Key,
                                Parent = item.Parent.Id,
                                Date = item.Date,
                                Deleted = item.Deleted,
                                Labels = item.Labels,
                                Notes = item.Notes
                            });
        }

        // PUT: api/Items/5
        [ResponseType(typeof(void))]
        //public IHttpActionResult PutItem(int id, DtoItem item)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != item.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(item).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ItemExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Items
        //[ResponseType(typeof(DtoItem))]
        //public IHttpActionResult PostItem(DtoItem item)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Items.Add(item);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = item.Id }, item);
        //}

        // DELETE: api/Items/5
        //[ResponseType(typeof(DtoItem))]
        //public IHttpActionResult DeleteItem(int id)
        //{
        //    Item item = db.Items.Find(id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Items.Remove(item);
        //    db.SaveChanges();

        //    return Ok(item);
        //}

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