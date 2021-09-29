using Alten.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Alten.API;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base (options)
    {
    }

    public DbSet<Reservation> Reservation { get; set; }
}