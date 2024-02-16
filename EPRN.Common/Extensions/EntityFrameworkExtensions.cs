using EPRN.Common.Data.DataModels;
using PrnStatus = EPRN.Common.Data.Enums.PrnStatus;

namespace EPRN.Common.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static IQueryable<PackagingRecoveryNote> ExcludeDeleted(this IQueryable<PackagingRecoveryNote> query)
        {
            return query.Where(prn => !prn.PrnHistory.Any(h => h.Status == (PrnStatus)Enums.PrnStatus.Deleted));
        }
    }
}
