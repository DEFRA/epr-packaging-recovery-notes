using EPRN.Common.Data.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrnStatus = EPRN.Common.Data.Enums.PrnStatus;


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

        public static List<SelectListItem> ToSelectList<TEnum>(this TEnum enumObj, string defaultItem, params TEnum[] members) where TEnum : Enum
        {
            var selectList = new List<SelectListItem>
            {
                new()
                {
                    Value = string.Empty,
                    Text = defaultItem
                }
            };
            var enumMembers = members is { Length: > 0 } ? members : Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            selectList.AddRange(enumMembers
                .Select(e => new SelectListItem
                {
                    Value = ((int)Convert.ChangeType(e, typeof(int))).ToString(),
                    Text = e.ToString()
                }));
            return selectList;
        }

        public static IQueryable<PackagingRecoveryNote> ExcludeDeleted(this IQueryable<PackagingRecoveryNote> query)
        {
            return query.Where(prn => !prn.PrnHistory.Any(h => h.Status == (PrnStatus)Enums.PrnStatus.Deleted));
        }
    }
}
