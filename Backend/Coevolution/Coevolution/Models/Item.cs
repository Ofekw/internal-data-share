using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Coevolution.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public virtual Item Parent { get; set; }
        public bool Deleted { get; set; }

        public List<Label> Labels { get; set; }
        public List<Note> Notes { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Item()
        {
            Labels = new List<Label>();
            Notes = new List<Note>();
        }

        public Item Updated()
        {
            this.UpdatedOn = DateTime.Now;
            return this;
        }

        public Item Created()
        {
            this.CreatedOn = DateTime.Now;
            Updated();
            return this;
        }

        public virtual DtoItem ToDto()
        {
            throw new NotImplementedException("ToDto should only be called on subclasses.");
        }

        public DtoItemReduced ToDtoReduced()
        {
            return new DtoItemReduced()
            {
                Id = this.Id,
                Key = this.Key,
                Labels = this.Labels.Select(label => label.Content).ToList(),
            };
        }
    }
}