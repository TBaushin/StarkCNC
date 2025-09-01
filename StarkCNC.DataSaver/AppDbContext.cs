using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;

namespace StarkCNC.DataSaver
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<BendingData> BendingDatas { get; set; }
    }
}
