using Microsoft.EntityFrameworkCore;
using campusfix.Models;

namespace campusfix.Data
{
    public class CampusFixContext : DbContext
    {
        public CampusFixContext(DbContextOptions<CampusFixContext> options)
            : base(options)
        {
        }

        public DbSet<Issue> Issues { get; set; }
    }
}