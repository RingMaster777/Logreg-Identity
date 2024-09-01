using System.ComponentModel.DataAnnotations;

namespace LogReg_Identity.Models.ViewModel
{
    public class NoteVM
    {
        [Required]
        public int NoteId { get; set; }

        [Required]
        [Display(Name ="Title")]
        public string? NoteTitle { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? NoteDescription { get; set; }
    }
}
