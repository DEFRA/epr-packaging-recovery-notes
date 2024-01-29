using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EPRN.Portal.Helpers.TagHelpers
{
    [HtmlTargetElement("input")]
    [HtmlTargetElement("textarea")]
    [HtmlTargetElement("select")]
    [HtmlTargetElement("checkbox")]
    [HtmlTargetElement("radio")]
    public class ReadOnlyTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Check the type attribute of the input element
            var typeAttribute = output.Attributes["type"];

            if (typeAttribute != null)
            {
                string inputType = typeAttribute.Value.ToString().ToLower();

                if (IsValidForDisabledAttribute(inputType))
                {
                    // It's a valid input type for the 'disabled' attribute
                    output.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    // It's not a valid type for 'disabled', so make it readonly
                    output.Attributes.Add("readonly", "readonly");
                }
            }
        }

        private bool IsValidForDisabledAttribute(string inputType)
        {
            // Define the input types for which 'disabled' is valid
            // Adjust this list based on your requirements
            string[] validDisabledTypes = { "text", "password", "checkbox", "radio" };

            return validDisabledTypes.Contains(inputType);
        }
    }
}
