using LogReg_Identity.Areas.Identity.Data;
using LogReg_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LogReg_Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    public DbSet<NoteModel> Notes { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed Permissions
        var permissions = new List<Permission>
        {
            new Permission { PermissionId = 1, PermissionName = "POST" },
            new Permission { PermissionId = 2, PermissionName = "GET" },
            new Permission { PermissionId = 3, PermissionName = "PATCH" },
            new Permission { PermissionId = 4, PermissionName = "DELETE" },
        };
        builder.Entity<Permission>().HasData(permissions);

        // Assuming the existing Admin role has an ID of "1" and User role has an ID of "2"
        // If the actual IDs are different, you need to replace them accordingly.

        // Seed RolePermissions
        var rolePermissions = new List<RolePermission>
        {
            new RolePermission {RpId=1 , RoleId = "4f5e2154-92cd-4ad8-95b3-82b1a1acfed3", PermissionId = 1 }, // Admin -> Create
            new RolePermission {RpId=2 ,  RoleId = "4f5e2154-92cd-4ad8-95b3-82b1a1acfed3", PermissionId = 2 }, // Admin -> Read
            new RolePermission {RpId=3,  RoleId = "4f5e2154-92cd-4ad8-95b3-82b1a1acfed3", PermissionId = 3 }, // Admin -> Update
            new RolePermission {RpId=4,  RoleId = "4f5e2154-92cd-4ad8-95b3-82b1a1acfed3", PermissionId = 4 }, // Admin -> Delete
            new RolePermission {RpId=5,  RoleId = "5bc96774-6718-44ff-857a-3644c8546ad0", PermissionId = 2 }, // User -> Read
        };
        builder.Entity<RolePermission>().HasData(rolePermissions);

 
    }
}
