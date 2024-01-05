using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using EPRN.Common.Data.DataModels;

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
