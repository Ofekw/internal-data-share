using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    //Leaf object, an item that does not have any children and has a value
    public class Leaf : Item
    {
        public string Value { get; set; }

        public Leaf () : base()
        {

        }

        //Object to DTO
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
                Type = "leaf",
                Parent = this.Parent.Id,
                Deleted = this.Deleted,
                Labels = this.Labels.Select(label => label.ToDto()).ToList(),
                Notes = DtoNote.NoteListToDtos(this.Notes),
                Value = this.Value,
                CreatedOn = this.CreatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                UpdatedOn = this.UpdatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                Path = this.Path();
            };
        }
    }
}