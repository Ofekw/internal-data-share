using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Coevolution.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        public Item Item { get; set; }
        public String Content { get; set; }
    }
}