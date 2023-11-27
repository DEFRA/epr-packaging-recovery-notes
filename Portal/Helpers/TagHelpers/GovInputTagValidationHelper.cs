using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Portal.Helpers.TagHelpers
{
    /// <summary>
    /// If the form fails validation:
    /// 
    /// This class goes through each of the specified tags specified below 
    /// checks to see if they are a govuk-form-group class, and if so
    /// adds the govuk-form-group--error class to the div for appropriate validation
    /// failure cases
    /// </summary>
    [HtmlTargetElement("div")]
    [HtmlTargetElement("radios")]
    public class GovInputTagValidationHelper : TagHelper
    {
        public override int Order => 2; 

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // can exit if valid as we don't need to do anything more
            if (ViewContext.ViewData.ModelState.IsValid)
                return;

            var outputClass = output.Attributes.FirstOrDefault(a => a.Name == "class");

            if (outputClass == null || !outputClass.Value.ToString().Contains("govuk-form-group", StringComparison.OrdinalIgnoreCase))
                return;

            output.Attributes.SetAttribute("class", $"{outputClass.Value} govuk-form-group--error");
        }
    }
}
