using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogReg_Identity.Models
{
    public class MenuPermissionModel
    {

        [Key]
        public int MenuPermissionId {  get; set; }


        [Required]
        public int MenuId { get; set; }
        [ForeignKey(nameof(MenuId))]
        [ValidateNever]
        public MenuModel Menu { get; set; }

        [Required]
        public string RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        [ValidateNever]
        public IdentityRole Role { get; set; }
    }
}
