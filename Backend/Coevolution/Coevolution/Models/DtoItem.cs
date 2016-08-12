using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    /// <summary>
    /// DTO for the Item class
    /// </summary>
    public class DtoItem
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
        /// Id of the parent Item
        /// </summary>
        public virtual int? Parent { get; set; }
        /// <summary>
        /// Daet of last modification of the Item in epoch
        /// </summary>
        public long Date { get; set; }
        /// <summary>
        /// Flag to represent "deleted" old/stale data
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// List of Item labels
        /// </summary>
        public List<string> Labels { get; set; }
        /// <summary>
        /// List of Item notes
        /// </summary>
        public List<string> Notes { get; set; }

        public DtoItem()
        {
            Labels = new List<String>();
            Notes = new List<String>();
        }
    }
}