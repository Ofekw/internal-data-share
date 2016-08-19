using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class Node : Item
    {
        public virtual List<Item> Children { get; set; }

        public Node() : base()
        {
            Children = new List<Item>();
        }

        public override DtoItem ToDto()
        {
            return new DtoNode()
            {
                Id = this.Id,
                Key = this.Key,
                Parent = this.Parent == null ? (int?)null : this.Parent.Id,
                Date = this.Date,
                Deleted = this.Deleted,
                Labels = this.Labels.Select(label => label.Content).ToList(),
                Notes = DtoNote.NoteListToDtos(this.Notes),
                Children = this.Children.Select<Item, int>(n => n.Id).ToList(),
                CreatedOn = this.CreatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                UpdatedOn = this.UpdatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
            };
        }
    }
}