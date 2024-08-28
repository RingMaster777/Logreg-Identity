using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LogReg_Identity.Models.ViewModel
{
    public class MenuVM
    {

        [Required]
        [Display(Name = "Menu Name")]
        public string? MenuName { get; set; }

        [Required]
        [Display(Name = "Parent")]
        public string? ParentName { get; set; }


        [Required]
        [Display(Name = "Assign To")]
        public List<string> AssignTo { get; set; }

    }
}
