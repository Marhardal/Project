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

        public DbSet<CategoryModel> Categories { get; set; }

        public DbSet<LocationModel> Locations { get; set; }

        public DbSet<ProjectModel> Projects { get; set; }

        public DbSet<ProjectLocation> ProjectLocations { get; set; }

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

            modelBuilder.Entity<CategoryModel>()
                .HasMany(p => p.Projects)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryID);

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


            modelBuilder.Entity<ProjectLocation>()
                .HasKey(pl => new { pl.ProjectID, pl.LocationID }); // composite key

            modelBuilder.Entity<ProjectLocation>()
                .HasOne(pl => pl.Project)
                .WithMany(p => p.ProjectLocations)
                .HasForeignKey(pl => pl.ProjectID);

            modelBuilder.Entity<ProjectLocation>()
                .HasOne(pl => pl.Location)
                .WithMany(l => l.ProjectLocations)
                .HasForeignKey(pl => pl.LocationID);


           // modelBuilder.Entity<ProjectModel>()
           //.HasIndex(p => p.LocationID)
           //.IsUnique(false);

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
                    Name = "Review",
                    Color = "#ffc107",
                    SortOrder = 2
                },
                new Status
                {
                    ID = Guid.Parse("c9a5f3d1-88b2-4a6e-91d3-7e4f2b6c8a33"),
                    Name = "Approved",
                    Color = "#8110b9",
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

            modelBuilder.Entity<CategoryModel>().HasData(
                 new CategoryModel
                 {
                     ID = Guid.Parse("a1b2c3d4-e5f6-7890-1234-56789abcdef0"),
                     Name = "Energy"
                 },
                 new CategoryModel
                 {
                     ID = Guid.Parse("0f1e2d3c-4b5a-6789-0123-456789abcdef"),
                     Name = "Agriculture"
                 }
            );

            modelBuilder.Entity<LocationModel>().HasData(
                new LocationModel { ID = Guid.Parse("94e36331-70d1-4188-b037-8fac1c3e1284"), Code = "MWBA", Location = "Balaka" },
                new LocationModel { ID = Guid.Parse("4fa6ea12-d660-4d4f-a2d4-f5ea09f33adf"), Code = "MWBL", Location = "Blantyre" },
                new LocationModel { ID = Guid.Parse("110ccd83-878e-4ac9-b5df-b0bd21e6ec69"), Code = "MWCK", Location = "Chikwawa" },
                new LocationModel { ID = Guid.Parse("671df2aa-1290-49ac-b8d6-e2f423e4c4ca"), Code = "MWCR", Location = "Chiradzulu" },
                new LocationModel { ID = Guid.Parse("3e45791f-5081-485b-8a54-4ced8ae265d2"), Code = "MWCT", Location = "Chitipa" },
                new LocationModel { ID = Guid.Parse("fb9dda42-ab5c-484f-acc0-5ff40ffbb34b"), Code = "MWDE", Location = "Dedza" },
                new LocationModel { ID = Guid.Parse("9988f408-d9a4-4b84-b07e-754ca0fb0a75"), Code = "MWDO", Location = "Dowa" },
                new LocationModel { ID = Guid.Parse("6a8bf09f-8950-4b80-a8fe-a1126002d500"), Code = "MWKR", Location = "Karonga" },
                new LocationModel { ID = Guid.Parse("17e9865f-4619-49bd-895b-dc30d858aa05"), Code = "MWKS", Location = "Kasungu" },
                new LocationModel { ID = Guid.Parse("9ee4ea79-a78a-4b8d-b968-a9c1abcc14d0"), Code = "MWLI", Location = "Lilongwe" },
                new LocationModel { ID = Guid.Parse("137cc546-1471-4a5e-a63b-e06ce14d532d"), Code = "MWLK", Location = "Likoma" },
                new LocationModel { ID = Guid.Parse("40129e87-f140-42e2-8dd3-b8a7b7fd2075"), Code = "MWMC", Location = "Mchinji" },
                new LocationModel { ID = Guid.Parse("700aa8b1-d143-4ef9-8058-68549198591b"), Code = "MWMG", Location = "Mangochi" },
                new LocationModel { ID = Guid.Parse("2d6fe440-8768-48c8-ad95-0eda33f36847"), Code = "MWMH", Location = "Machinga" },
                new LocationModel { ID = Guid.Parse("856f67f5-ab9b-4b69-96a4-5299f423a50b"), Code = "MWMU", Location = "Mulanje" },
                new LocationModel { ID = Guid.Parse("1e65b44d-f01f-4cc3-bde9-c41b94e60c8f"), Code = "MWMW", Location = "Mwanza" },
                new LocationModel { ID = Guid.Parse("1d54de55-d589-45b3-81df-3b01a35f8a87"), Code = "MWMZ", Location = "Mzimba" },
                new LocationModel { ID = Guid.Parse("f70940f3-3450-4b53-9f06-7225399be7d3"), Code = "MWNB", Location = "Nkhata Bay" },
                new LocationModel { ID = Guid.Parse("e61a637c-1c66-4a19-bd7d-32ccbebf6d42"), Code = "MWNE", Location = "Neno" },
                new LocationModel { ID = Guid.Parse("420bc4b1-789d-4553-83d2-f2d14007275f"), Code = "MWNT", Location = "Ntchisi" },
                new LocationModel { ID = Guid.Parse("c871ca5e-3179-4c09-afd9-0ad9a17a5ffc"), Code = "MWNK", Location = "Nkhotakota" },
                new LocationModel { ID = Guid.Parse("988b7aac-7970-4d4a-b6b2-8daabc7c844b"), Code = "MWNS", Location = "Nsanje" },
                new LocationModel { ID = Guid.Parse("90901f0f-46af-447a-b971-313f77e4eb59"), Code = "MWNU", Location = "Ntcheu" },
                new LocationModel { ID = Guid.Parse("c92cd76c-cf7e-4548-a125-b4e7f0b48929"), Code = "MWPH", Location = "Phalombe" },
                new LocationModel { ID = Guid.Parse("683873af-1116-4a1f-8fed-8d82d19a86b5"), Code = "MWRU", Location = "Rumphi" },
                new LocationModel { ID = Guid.Parse("d8dc2004-9d09-45c8-8120-fd2d6a1852c8"), Code = "MWSA", Location = "Salima" },
                new LocationModel { ID = Guid.Parse("0a57923d-1ee7-4ce8-a392-5c3209b02170"), Code = "MWTH", Location = "Thyolo" },
                new LocationModel { ID = Guid.Parse("bbe4fa2b-70cc-40f2-bcee-c2236d6e6499"), Code = "MWZO", Location = "Zomba" }
            );
        }
    }
}
