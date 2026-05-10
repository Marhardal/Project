using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class DBContext : IdentityDbContext<IdentityUser>
    {
        public DBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Proponent> Proponents { get; set; }

        public DbSet<ContactPerson> Contacts { get; set; }

        public DbSet<ProjectModel> Projects { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<TrackingModel> Trackings { get; set; }

        public DbSet<ReviewModel> Reviews { get; set; }

        public DbSet<UserModel> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Proponent>()
                .HasMany(c => c.Contacts)
                .WithOne(c => c.Proponent)
                .HasForeignKey(c => c.ProponentID);

            modelBuilder.Entity<Proponent>()
                .HasMany(p => p.Projects)
                .WithOne(p => p.Proponent)
                .HasForeignKey(p => p.ProponentID);

            // Project → Trackings
            modelBuilder.Entity<ProjectModel>()
                .HasMany(p => p.Trackings)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectID);

            // Status → Trackings
            modelBuilder.Entity<Status>()
                .HasMany(s => s.Trackings)
                .WithOne(t => t.Status)
                .HasForeignKey(t => t.StatusID);

            // Tracking → Review (1:1)
            modelBuilder.Entity<TrackingModel>()
                .HasOne(t => t.Review)
                .WithOne(r => r.Tracking)
                .HasForeignKey<ReviewModel>(r => r.TrackingID);

            // User → Trackings
            modelBuilder.Entity<TrackingModel>()
                .HasOne(t => t.User)
                .WithMany(u => u.Trackings)
                .HasForeignKey(t => t.userID)
                .HasPrincipalKey(u => u.UserID);
                


            modelBuilder.Entity<UserModel>()
    .HasOne(u => u.identityUser)
    .WithOne()
    .HasForeignKey<UserModel>(u => u.UserID)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Status>().HasData(
    new Status
    {
        ID = Guid.Parse("a3f1c9e2-7b4d-4c91-8f2a-1d6e9b7c3f10"),
        Name = "Submitted",
        Color = "#6c757d",
        SortOrder = 1
    },
    new Status
    {
        ID = Guid.Parse("b7e2d4f9-3a11-42c7-9c8d-5f2a6b1e4d22"),
        Name = "Under Review",
        Color = "#ffc107",
        SortOrder = 2
    },
    new Status
    {
        ID = Guid.Parse("c9a5f3d1-88b2-4a6e-91d3-7e4f2b6c8a33"),
        Name = "Approved",
        Color = "#198754",
        SortOrder = 3
    },
    new Status
    {
        ID = Guid.Parse("d1f7b3a9-55c4-4f8a-b2e1-9a6c3d7e5f44"),
        Name = "Rejected",
        Color = "#dc3545",
        SortOrder = 4
    },
    new Status
    {
        ID = Guid.Parse("e4c2a8f6-19d3-4b7f-a9c2-3f8d6e1b2a55"),
        Name = "Assigned",
        Color = "#0d6efd",
        SortOrder = 5
    },
    new Status
    {
        ID = Guid.Parse("f6a8c2e1-11d3-4a9f-b7c2-3d8e6f1a2b66"),
        Name = "In Progress",
        Color = "#0dcaf0",
        SortOrder = 6
    },
    new Status
    {
        ID = Guid.Parse("f7b9d3f2-22e4-4c1a-8b5f-9e6d3c2a1f77"),
        Name = "Completed",
        Color = "#157347",
        SortOrder = 7
    }
);
        }
    }
}
