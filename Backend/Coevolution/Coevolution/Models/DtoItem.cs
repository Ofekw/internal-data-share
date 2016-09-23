using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    /// <summary>
    /// DTO for the Item class
    /// </summary>
    [JsonConverter(typeof(DtoItemJsonConverter))]
    public class DtoItem
    {
        /// <summary>
        /// Id for storage in DB
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Name of Item
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Whether the item is a node or a leaf
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Id of the parent Item
        /// </summary>
        public virtual int? Parent { get; set; }
        /// <summary>
        /// Flag to represent "deleted" old/stale data
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// List of Item labels
        /// </summary>
        public List<DtoLabel> Labels { get; set; }
        /// <summary>
        /// List of Item notes
        /// </summary>
        public List<DtoNote> Notes { get; set; }
        /// <summary>
        /// Date item was create in ISO 8601 Format
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// Last DateTime item was updated in ISO 8601 Format
        /// </summary>
        public string UpdatedOn { get; set; }

        public List<KeyValuePair<int, string>> Path { get; set; }

        public DtoItem()
        {
            Labels = new List<DtoLabel>();
            Notes = new List<DtoNote>();
        }

        //Convert DTO to domain object
        public virtual Item ToDomainObject(Node parent){
            throw new NotImplementedException("ToDomainObject should only be called on subclasses.");
        }

        //Converter for items depending on node or leaf type
        private class DtoItemJsonConverter : JsonCreationConverter<DtoItem>
        {
            protected override DtoItem Create(Type objectType, JObject jObject)
            {
                var type = jObject.Value<string>("Type");
                if (type == null)
                {
                    throw new InvalidDataException("DtoItem sent by client must have non-null type field.");
                }
                else if (type.Equals("node"))
                {
                    return new DtoNode();
                }
                else if (type.Equals("leaf"))
                {
                    return new DtoLeaf();
                }
                else
                {
                    throw new InvalidDataException("DtoItem sent by client must have type field set to 'node' or 'leaf'."); // TODO: Throw 4XX rather than 5XX
                }
            }
        }
    }
}