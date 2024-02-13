using System.Globalization;

namespace EPRN.Portal.Helpers
{
    public class StringHelper
    {
        public static string ToTitleCase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            
            var words = str.Split(' ');
            
            for (var i = 0; i < words.Length; i++)
                if (i == 0)
                    // Capitalize the first word
                    words[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i].ToLower());
                else
                    // Lowercase the rest of the words
                    words[i] = words[i].ToLower();
            
            return string.Join(' ', words);
        }
    }
}
