using Microsoft.EntityFrameworkCore;
using WasteManagement.API.Models;

namespace WasteManagement.API.Data
{
    public class WasteContext : DbContext
    {
        public WasteContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DefaultTable> DefaultTable { get; set; }
    }
}
