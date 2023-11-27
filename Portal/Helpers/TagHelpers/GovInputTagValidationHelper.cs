using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Portal.Helpers.TagHelpers
{
    // class name govuk-form-group
    [HtmlTargetElement("radios")]
    public class GovInputTagValidationHelper : TagHelper
    {
        public override int Order => 2; 

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            int i = 0;
            //if (!context.AllAttributes.Any(a => a.Name != "class" && a.Value.ToString() != "govuk-form-group"))
            //    return;
            if (context.AllAttributes.Any(a => a.Value.ToString() == "govuk-form-group"))
                i = 0;

            if (ViewContext.ViewData.ModelState.IsValid)
                return;

            var id = context.AllAttributes.FirstOrDefault(a => a.Name.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value.ToString();

            if (string.IsNullOrWhiteSpace(id))
                return;
            sdfgd
            // add class govuk-form-group--error to matching div id
        }
    }
}
