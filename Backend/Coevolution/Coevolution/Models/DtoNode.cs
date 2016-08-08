using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoNode : DtoItem
    {
        public virtual List<int> Children { get; set; }

        public DtoNode()
            : base()
        {
            Children = new List<int>();
        }
    }
}