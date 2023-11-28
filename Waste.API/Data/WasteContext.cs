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

        public virtual DbSet<WasteType> WasteTypes { get; set; }

        public virtual DbSet<WasteSubType> WasteSubTypes { get; set; }

        public virtual DbSet<WasteJourney> WasteJourneys { get; set; }
    }
}
