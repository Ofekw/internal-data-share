using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    //Node object, an item with child items
    public class Node : Item
    {
        public virtual List<Item> Children { get; set; }

        public string Comment { get; set; }

        public Node() : base()
        {
            Children = new List<Item>();
        }

        //Object to DTO
        public override DtoItem ToDto()
        {
            return new DtoNode()
            {
                Id = this.Id,
                Key = this.Key,
                Type = "node",
                Parent = this.Parent == null ? (int?)null : this.Parent.Id,
                Deleted = this.Deleted,
                Comment = this.Comment,
                Labels = this.Labels.Select(label => label.ToDto()).ToList(),
                Notes = DtoNote.NoteListToDtos(this.Notes),
                LeafChildren = this.Children.Where(n => n is Leaf).Select(n => (DtoLeaf) n.ToDto()).ToList(),
                NodeChildren = this.Children.Where(n => n is Node).Select(n => n.ToDtoReduced()).ToList(),
                CreatedOn = this.CreatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                UpdatedOn = this.UpdatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                Path = this.Path()
            };
        }
    }
}