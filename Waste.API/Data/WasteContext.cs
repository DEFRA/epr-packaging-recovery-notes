using Microsoft.EntityFrameworkCore;
using Waste.API.Models;

namespace WasteManagement.API.Data
{
    public class WasteContext : DbContext
    {
        public WasteContext()
        {
        }

        public WasteContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<WasteType> WasteType { get; set; }

        public DbSet<WasteSubType> WasteSubType { get; set; }

        public DbSet<WasteJourney> WasteJourney { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("WasteConnectionString");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
