using System.ComponentModel.DataAnnotations;

namespace LogReg_Identity.Models
{
    public class NoteModel
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        public string? NoteTitle { get; set; }

        [Required]
        public string? NoteDescription { get; set; }

    }
}
