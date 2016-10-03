using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoSearchItem
    {
        /// <summary>
        /// Id of the item
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Whether the item is a leaf, or a node
        /// </summary>
        public bool IsLeaf { get; set; }
        /// <summary>
        /// Path of the item for breadcrumb use
        /// </summary>
        public List<KeyValuePair<int, string>> Path { get; set; }
        /// <summary>
        /// Key for title of this item
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Value associated with key
        /// </summary>
        public string Value { get; set; }

        public DtoSearchItem()
        {

        }

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