using Alten.API.Models;
using Microsoft.EntityFrameworkCore;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base (options)
    {
    }

    public DbSet<Reservation> Reservation { get; set; }
}