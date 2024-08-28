using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogReg_Identity.Models
{
    public class RolePermissionModel
    {
        [Key]
        public int RpId { get; set; }

        
        public string RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        [ValidateNever]
        public IdentityRole Role { get; set; }

        public int PermissionId { get; set; }

        [ForeignKey(nameof(PermissionId))]
        [ValidateNever]
        public PermissionModel Permission { get; set; }
    }

}
