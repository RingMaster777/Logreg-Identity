using LogReg_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LogReg_Identity.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<PermissionModel> Permissions { get; set; }
    public DbSet<RolePermissionModel> RolePermissions { get; set; }

    public DbSet<NoteModel> Notes { get; set; }

    public DbSet<MenuModel> Menus { get; set; }

    public DbSet<MenuPermissionModel> MenuPermissions { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed Permissions
        var permissions = new List<PermissionModel>
        {
            new PermissionModel { PermissionId = 1, PermissionName = "POST" },
            new PermissionModel { PermissionId = 2, PermissionName = "GET" },
            new PermissionModel { PermissionId = 3, PermissionName = "PATCH" },
            new PermissionModel { PermissionId = 4, PermissionName = "DELETE" },
        };
        builder.Entity<PermissionModel>().HasData(permissions);

        // Assuming the existing Admin role has an ID of "1" and User role has an ID of "2"
        // If the actual IDs are different, you need to replace them accordingly.

        // Seed RolePermissions
        var rolePermissions = new List<RolePermissionModel>
        {
            new RolePermissionModel {RpId=1 , RoleId = "4f5e2154-92cd-4ad8-95b3-82b1a1acfed3", PermissionId = 1 }, // Admin -> Create
            new RolePermissionModel {RpId=2 ,  RoleId = "4f5e2154-92cd-4ad8-95b3-82b1a1acfed3", PermissionId = 2 }, // Admin -> Read
            new RolePermissionModel {RpId=3,  RoleId = "4f5e2154-92cd-4ad8-95b3-82b1a1acfed3", PermissionId = 3 }, // Admin -> Update
            new RolePermissionModel {RpId=4,  RoleId = "4f5e2154-92cd-4ad8-95b3-82b1a1acfed3", PermissionId = 4 }, // Admin -> Delete
            new RolePermissionModel {RpId=5,  RoleId = "5bc96774-6718-44ff-857a-3644c8546ad0", PermissionId = 2 }, // User -> Read
        };
        builder.Entity<RolePermissionModel>().HasData(rolePermissions);

 
    }
}
