using Microsoft.EntityFrameworkCore;
using BloodyAPI.Models;

namespace BloodyAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
        {

        }


        public DbSet<User> Users { get; set; }
    }
}
