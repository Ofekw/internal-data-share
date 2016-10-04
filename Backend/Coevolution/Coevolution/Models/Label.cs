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

        public Label()
        {
            this.Items = new List<Item>();
        }

        public DtoLabel ToDto()
        {
            return new DtoLabel()
            {
                Id = this.Id,
                Content = this.Content
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is Label)
            {
                return this.Id == ((Label)obj).Id;
            } else
            {
                return false;
            }
        }
    }
}