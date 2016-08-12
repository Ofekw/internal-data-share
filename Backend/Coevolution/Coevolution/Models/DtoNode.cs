using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoNode : DtoItem
    {
        /// <summary>
        /// List of Ids that define the child nodes
        /// </summary>
        public virtual List<int> Children { get; set; }

        public DtoNode()
            : base()
        {
            Children = new List<int>();
        }
    }
}