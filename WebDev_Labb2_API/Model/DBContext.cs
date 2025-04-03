using Microsoft.EntityFrameworkCore;

namespace WebDev_Labb2_API.Model
{
    public class DBContext : DbContext
    {
        public DbSet<Products> Products { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Orders> Orders { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }
    }
}
