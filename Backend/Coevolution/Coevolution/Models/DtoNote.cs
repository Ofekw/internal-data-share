using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coevolution.Models
{
    /// <summary>
    /// DTO for the note class
    /// </summary>
    [JsonConverter(typeof(DtoNote.DtoNoteConverter))]
    public class DtoNote
    {
        /// <summary>
        /// DB Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Content of the note
        /// </summary>
        public String Content { get; set; }
        /// <summary>
        /// Date the note was created
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// Date the note was last updated
        /// </summary>
        public string UpdatedOn { get; set; }

        public DtoNote()
        {
            
        }

        public virtual Note ToDomainObject()
        {
            var newNote = new Note()
            {
                Id = this.Id,
                Content = this.Content,
                CreatedOn = DateTime.Parse(this.CreatedOn, null, System.Globalization.DateTimeStyles.RoundtripKind),
                UpdatedOn = DateTime.Parse(this.UpdatedOn, null, System.Globalization.DateTimeStyles.RoundtripKind)
            };
            return newNote;
        }

        public static List<Note> DtoNoteListToDomainObjecs(List<DtoNote> noteDtos)
        {
            List<Note> notes = new List<Note>();
            foreach (var note in noteDtos)
            {
                notes.Add(note.ToDomainObject());
            }
            return notes;
        }

        public static List<DtoNote> NoteListToDtos(List<Note> notes)
        {
            List<DtoNote> noteDtos = new List<DtoNote>();
            foreach (var note in notes)
            {
                noteDtos.Add(note.ToDto());
            }
            return noteDtos;
        }

        private class DtoNoteConverter : JsonCreationConverter<DtoNote>
        {
            protected override DtoNote Create(Type objectType, JObject jObject)
            {
                var type = jObject.Value<string>("Type");
                return new DtoNote();
            }
        }
    }
}