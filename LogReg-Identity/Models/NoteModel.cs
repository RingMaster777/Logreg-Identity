using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        public string NoteAuthor { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string CreatorId { get; set; } = null;

        [ForeignKey(nameof(CreatorId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
