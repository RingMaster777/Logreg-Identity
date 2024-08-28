using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogReg_Identity.Models
{
    public class MenuModel
    {
        [Key]
        public int MenuId { get; set; }

        [Required]
        public string? MenuName { get; set; }

        [Required]
        public int? MenuParentId { get; set; }

    }
}
