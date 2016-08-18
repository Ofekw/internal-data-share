using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoLeaf : DtoItem
    {
        /// <summary>
        /// Value of the property defined by the key
        /// </summary>
        public string Value { get; set; }

        public DtoLeaf()
            : base()
        {

        }

        public override Item ToDomainObject(Node parent)
        {
            var newLeaf = new Leaf()
            {
                Key = this.Key,
                Parent = parent,
                Date = this.Date,
                Deleted = this.Deleted,
                Value = this.Value
            };

            if (parent == null)
            {
                throw new InvalidDataException("Leaf must have a parent."); // TODO: Throw 4XX rather than 5XX
            }

            parent.Children.Add(newLeaf);

            return newLeaf;
        }
    }
}