using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    [JsonConverter(typeof(DtoItemJsonConverter))]
    public class DtoItem
    {
        public int? Id { get; set; }
        public string Key { get; set; }
        public virtual int? Parent { get; set; }
        public long Date { get; set; }
        public bool Deleted { get; set; }

        public List<string> Labels { get; set; }
        public List<string> Notes { get; set; }

        public DtoItem()
        {
            Labels = new List<String>();
            Notes = new List<String>();
        }

        public virtual Item ToDomainObject(Node parent){
            throw new NotImplementedException("ToDomainObject should only be called on subclasses.");
        }

        private class DtoItemJsonConverter : JsonCreationConverter<DtoItem>
        {
            protected override DtoItem Create(Type objectType, JObject jObject)
            {
                var type = jObject.Value<string>("Type");
                if (type == null)
                {
                    throw new InvalidDataException("DtoItem sent by client must have non-null type field."); // TODO: Throw 4XX rather than 5XX
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