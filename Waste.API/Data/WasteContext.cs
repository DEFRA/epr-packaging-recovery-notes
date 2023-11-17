using Microsoft.EntityFrameworkCore;
using Waste.API.Models;

namespace WasteManagement.API.Data
{
    public class WasteContext : DbContext
    {
        public WasteContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<WasteType> WasteType { get; set; }

        public DbSet<WasteSubType> WasteSubType { get; set; }

        public DbSet<WasteJourney> WasteJourney { get; set; }
    }
}
