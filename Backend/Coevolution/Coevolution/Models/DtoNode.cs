using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoNode : DtoItem
    {
        /// <summary>
        /// List of Ids that define the child nodes
        /// </summary>
        public virtual List<int> Children { get; set; }

        public DtoNode()
            : base()
        {
            Children = new List<int>();
        }

        public override Item ToDomainObject(Node parent)
        {
            var newNode = new Node()
            {
                Key = this.Key,
                Parent = parent,
                Date = this.Date,
                Deleted = this.Deleted,
                Notes = DtoNote.DtoNoteListToDomainObjecs(this.Notes),
                CreatedOn = DateTime.Parse(this.CreatedOn, null, System.Globalization.DateTimeStyles.RoundtripKind),
                UpdatedOn = DateTime.Parse(this.UpdatedOn, null, System.Globalization.DateTimeStyles.RoundtripKind)
            };

            if (parent != null)
            {
                parent.Children.Add(newNode);
            }

            return newNode;
        }
    }
}