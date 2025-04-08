using Microsoft.EntityFrameworkCore;
using Proyect_Solo_Recommendation.Models;

namespace Proyect_Solo_Recommendation.Data
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
   
}
