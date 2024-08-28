using System.ComponentModel.DataAnnotations;

namespace LogReg_Identity.Models
{
    public class PermissionModel
    {
        [Key]
        public int PermissionId { get; set; }

        [Required]
        public string? PermissionName { get; set; }
    }

}
