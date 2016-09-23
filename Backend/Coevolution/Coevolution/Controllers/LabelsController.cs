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
using System.Web.Http.Cors;

namespace Coevolution.Controllers
{
    //Enable Cross Origin Resource Sharing for local development
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class LabelsController : ApiController
    {
        private ModelContext db = new ModelContext();

        // GET: api/Labels
        /// <summary>
        /// Get a list of all Labels
        /// </summary>
        public IQueryable<Label> GetLabels()
        {
            return db.Labels;
        }

        // POST: api/Labels
        /// <summary>
        /// Add a new Label to the database
        /// </summary>
        [ResponseType(typeof(DtoLabel))]
        public IHttpActionResult PostLabel(DtoLabel dtoLabel)
        {
            //Validate supplied DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Label label = dtoLabel.ToDomainObject();

            db.Labels.Add(label);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = label.Id }, label.ToDto());
        }

        // DELETE: api/Labels/5
        /// <summary>
        /// Remove a label with the specified Id from the database
        /// </summary>
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteLabel(int id)
        {
            //Find item with id
            Label label = db.Labels.Include("Items").First(u => u.Id == id);
            if (label == null)
            {
                return NotFound();
            }
            foreach (Item item in label.Items)
            {
                item.Labels.Remove(label);
            }
            db.Labels.Remove(label);
            db.SaveChanges();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LabelExists(int id)
        {
            return db.Labels.Count(e => e.Id == id) > 0;
        }
    }
}