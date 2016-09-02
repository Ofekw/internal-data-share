using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoItemReduced
    {
        /// <summary>
        /// Id for storage in DB
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of Item
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// List of Item labels
        /// </summary>
        public List<string> Labels { get; set; }
    }
}