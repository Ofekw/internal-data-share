using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class Leaf : Item
    {
        public string Value { get; set; }

        public Leaf () : base()
        {

        }

        public override DtoItem ToDto()
        {
            if (this.Parent == null)
            {
                throw new NullReferenceException("Leaf's parent cannot be null.");
            }
            return new DtoLeaf()
            {
                Id = this.Id,
                Key = this.Key,
                Parent = this.Parent.Id,
                Date = this.Date,
                Deleted = this.Deleted,
                Labels = this.Labels.Select(label => label.Content).ToList(),
                Notes = this.Notes.Select(note => note.Content).ToList(),
                Value = this.Value
            };
        }
    }
}