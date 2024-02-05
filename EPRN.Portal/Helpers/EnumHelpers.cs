using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPRN.Portal.Helpers
{
    public static class EnumHelpers
    {
        public static List<SelectListItem> ToSelectList<TEnum>(Type enumType, string defaultItem, params TEnum[] members) where TEnum : Enum
        {
            var selectList = new List<SelectListItem>
            {
                new()
                {
                    Value = string.Empty,
                    Text = defaultItem
                }
            };
            var enumMembers = members.Length > 0 ? members : Enum.GetValues(enumType).Cast<TEnum>();
        
            selectList.AddRange(enumMembers
                .Select(e => new SelectListItem
                {
                    Value = ((int)Convert.ChangeType(e, typeof(int))).ToString(),
                    Text = e.ToString()
                }));
        
            return selectList;
        }
    }
}