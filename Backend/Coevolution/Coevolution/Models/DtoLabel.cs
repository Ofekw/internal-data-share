using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoLabel
    {
        /// <summary>
        /// Id of the label
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// String content of the label
        /// </summary>
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