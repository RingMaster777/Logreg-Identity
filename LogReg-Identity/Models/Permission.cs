using System.ComponentModel.DataAnnotations;

namespace LogReg_Identity.Models
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        [Required]
        public string? PermissionName { get; set; }
    }

}
