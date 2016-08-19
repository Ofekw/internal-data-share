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
        public List<Item> Items { get; set; }
        public String Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public DtoNote ToDto()
        {
            return new DtoNote()
            {
                Id = this.Id,
                Content = this.Content,
                CreatedOn = this.CreatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                UpdatedOn = this.UpdatedOn.ToString("s", System.Globalization.CultureInfo.InvariantCulture)
            };
        }
    }
}