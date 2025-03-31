using Microsoft.EntityFrameworkCore;

namespace WebDev_Labb2_API.Model
{
    public class DBContext : DbContext
    {
        public DbSet<Products> Products { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Orders> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new ConfigurationBuilder().AddUserSecrets<Program>().Build().GetConnectionString("MongoDb");

            var collection = "WebDev_Labb2_DB";

            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

            optionsBuilder.UseMongoDB(connectionString, collection);
        }
    }
}
