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
    }
}