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
            return (tonnage >= 0.01 && tonnage <= 0.5) || (tonnage != 0) ? false : true;
        }
    }
}
