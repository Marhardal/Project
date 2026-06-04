using Microsoft.EntityFrameworkCore;

public class NewDbContext(DbContextOptions<NewDbContext> options) : DbContext(options)
{
    public DbSet<Project.Models.LocationModel> LocationModel { get; set; } = default!;
}
