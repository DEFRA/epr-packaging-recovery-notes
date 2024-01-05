using EPRN.PRNS.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPRN.PRNS.API.Data
{
    [ExcludeFromCodeCoverage]
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
