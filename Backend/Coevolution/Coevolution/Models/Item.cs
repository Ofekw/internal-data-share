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
        public long Date { get; set; }
        public bool Deleted { get; set; }

        public List<string> Labels { get; set; }
        public List<string> Notes { get; set; }

        public Item()
        {
            Labels = new List<String>();
            Notes = new List<String>();
        }
    }
}