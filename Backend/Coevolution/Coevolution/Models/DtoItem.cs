using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoItem
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public virtual int Parent { get; set; }
        public long Date { get; set; }
        public bool Deleted { get; set; }

        public List<string> Labels { get; set; }
        public List<string> Notes { get; set; }

        public DtoItem()
        {
            Labels = new List<String>();
            Notes = new List<String>();
        }
    }
}