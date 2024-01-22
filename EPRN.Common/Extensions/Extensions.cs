namespace EPRN.Common.Extensions
{
    public static class Extensions
    {
        public static string RadioButtonDateString(this int key)
        {
            return $"Month{key}";
        }
        
        public static bool IsFeb29(this DateTime date)
        {
            return DateTime.IsLeapYear(date.Year) && date is { Month: 2, Day: 29 };
        }      
    }
}
