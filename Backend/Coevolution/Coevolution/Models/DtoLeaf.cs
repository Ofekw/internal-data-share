using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoLeaf : DtoItem
    {
        /// <summary>
        /// Value of the property defined by the key
        /// </summary>
        public string Value { get; set; }

        public DtoLeaf()
            : base()
        {

        }
    }
}