using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoLeaf : DtoItem
    {
        public string Value { get; set; }

        public DtoLeaf()
            : base()
        {

        }
    }
}