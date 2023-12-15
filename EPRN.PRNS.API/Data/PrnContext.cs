using EPRN.PRNS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EPRN.PRNS.API.Data
{
    public class PrnContext : DbContext
    {
        public PrnContext()
        {
        }

        public PrnContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<PackagingRecoveryNote> PRN { get; set; }
    }
}
