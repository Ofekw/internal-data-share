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
        {}

        //DTO to domain object
        public virtual Note ToDomainObject()
        {
            var newNote = new Note(this.Content)
            {
                Id = this.Id,
                CreatedOn = DateTime.Parse(this.CreatedOn, null, System.Globalization.DateTimeStyles.RoundtripKind),
                UpdatedOn = DateTime.Parse(this.UpdatedOn, null, System.Globalization.DateTimeStyles.RoundtripKind)
            };
            return newNote;
        }

        //List of DTOs to domain objects
        public static List<Note> DtoNoteListToDomainObjecs(List<DtoNote> noteDtos)
        {
            List<Note> notes = new List<Note>();
            foreach (var note in noteDtos)
            {
                notes.Add(note.ToDomainObject());
            }
            return notes;
        }

        //List of notes to DTOs
        public static List<DtoNote> NoteListToDtos(List<Note> notes)
        {
            List<DtoNote> noteDtos = new List<DtoNote>();
            foreach (var note in notes)
            {
                noteDtos.Add(note.ToDto());
            }
            return noteDtos;
        }
    }
}
