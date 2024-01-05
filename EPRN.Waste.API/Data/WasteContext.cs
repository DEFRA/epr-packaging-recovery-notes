using EPRN.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace EPRN.Waste.API.Data
{
    public class WasteContext : DbContext
    {
        public WasteContext()
        {
        }

        public WasteContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<WasteType> WasteType { get; set; }

        public virtual DbSet<WasteSubType> WasteSubType { get; set; }

        public virtual DbSet<WasteJourney> WasteJourney { get; set; }
    }
}
