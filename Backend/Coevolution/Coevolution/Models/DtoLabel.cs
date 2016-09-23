using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoLabel
    {
        public int? Id { get; set; }
        public String Content { get; set; }

        public Label ToDomainObject()
        {
            Label label = new Label
            {
                Content = this.Content
            };
            return label;
        }
    }
}