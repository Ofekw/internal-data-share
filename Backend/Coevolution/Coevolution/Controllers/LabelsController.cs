﻿using System;
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