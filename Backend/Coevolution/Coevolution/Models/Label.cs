using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Coevolution.Models
{
    //Label object, used as tags to organise items
    public class Label
    {
        [Key]
        public int Id { get; set; }
        public List<Item> Items { get; set; }
        public String Content { get; set; }
    }
}