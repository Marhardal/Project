using Microsoft.EntityFrameworkCore;

public class NewDbContext(DbContextOptions<NewDbContext> options) : DbContext(options)
{
    public DbSet<Project.Models.TasksModel> TasksModel { get; set; } = default!;
}
