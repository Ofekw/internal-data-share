using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    //DTO for node items
    public class DtoNode : DtoItem
    {
        /// <summary>
        /// List of Ids that define the child leaf items
        /// </summary>
        public virtual List<DtoLeaf> LeafChildren { get; set; }

        /// <summary>
        /// List of Ids that define the child node items
        /// </summary>
        public virtual List<DtoItemReduced> NodeChildren { get; set; }

        public DtoNode()
            : base()
        {
            LeafChildren = new List<DtoLeaf>();
            NodeChildren = new List<DtoItemReduced>();
        }

        //DTO to domain object
        public override Item ToDomainObject(Node parent)
        {
            var newNode = new Node()
            {
                Key = this.Key,
                Parent = parent,
                Deleted = this.Deleted,
                Notes = DtoNote.DtoNoteListToDomainObjecs(this.Notes),
                //CreatedOn = DateTime.Parse(this.CreatedOn, null, System.Globalization.DateTimeStyles.RoundtripKind),
                //UpdatedOn = DateTime.Parse(this.UpdatedOn, null, System.Globalization.DateTimeStyles.RoundtripKind)
            };

            return newNode;
        }
    }
}