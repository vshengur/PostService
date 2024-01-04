using Microsoft.EntityFrameworkCore;
using PostService.Domain.Models;

namespace PostService.Domain.DbContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts { get; set; }
}
