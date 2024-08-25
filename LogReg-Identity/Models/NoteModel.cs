using LogReg_Identity.Areas.Identity.Data;
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

    }
}
