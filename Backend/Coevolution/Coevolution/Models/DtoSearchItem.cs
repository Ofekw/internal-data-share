using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoSearchItem
    {

        public int Id { get; set; }

        public bool IsLeaf { get; set; }

        public List<KeyValuePair<int, string>> Path { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public DtoSearchItem(Item item)
        {
            IsLeaf = item is Leaf;
            Id = IsLeaf ? item.Parent.Id : item.Id;
            Path = item.Path();
            Key = item.Key;
            Value = IsLeaf ? ((Leaf)item).Value : null;
        }
    }
}