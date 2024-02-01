using EPRN.Common.Enums;

namespace EPRN.Portal.Helpers.Extensions
{
    public static class ViewExtensions
    {
        public static bool DisplayLinkForTonnage(this double tonnage)
        {
            return tonnage <= 0.5 ? false : true;
        }
        public static bool DisplayWarningForTonnage(this double tonnage)
        {
            return (tonnage >= 0.01 && tonnage <= 0.5) || (tonnage != 0) ? true : false;
        }

        public static CancelPermission GetCancelPermission(this PrnStatus status)
        {
            // if a PRN has been cancelled or a request to cancel, then
            // we cannot cancel either way
            if (status == PrnStatus.Cancelled ||
                status == PrnStatus.CancellationRequested)
                return CancelPermission.NotAllowed;

            // if a PRN has been accepted, we can only request to cancel
            if (status == PrnStatus.Accepted)
                return CancelPermission.RequestToCancelAllowed;

            // if a PRN has not yet been accepted 

            return CancelPermission.CancelAllowed;
        }
    }
}
