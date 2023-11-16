using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;

namespace PRN.Web.Helpers.TagHelpers
{
    [HtmlTargetElement("radios", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RadioTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-title")]
        public string Title { get; set; }

        public ModelExpression? AspFor { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;

        public RadioTagHelper(
            IHtmlGenerator generator) : base()
        {
            _generator = generator;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (AspFor != null)
            {
                var type = AspFor.Metadata.ModelType;
                var underlyingType = Nullable.GetUnderlyingType(type);

                if (underlyingType != null)
                {
                    type = underlyingType;
                }

                if (!type.IsEnum && type != typeof(bool) && type.GetInterface(nameof(IEnumerable)) == null)
                {
                    return;
                }

                output.TagName = "div";
                output.Attributes.Add("class", "govuk-form-group");


                var container = new TagBuilder("fieldset");
                container.AddCssClass("govuk-fieldset");
                container.InnerHtml.AppendHtml(AddTitle());

                var radioOptionContainer = new TagBuilder("div");
                radioOptionContainer.AddCssClass("govuk-radios");
                radioOptionContainer.Attributes.Add("data-module", "govuk-radios");

                if (type == typeof(bool))
                {
                    radioOptionContainer.InnerHtml.AppendHtml(AddRadioOption(AspFor.Name, true, AspFor.Model));
                    radioOptionContainer.InnerHtml.AppendHtml(AddRadioOption(AspFor.Name, false, AspFor.Model));
                }
                else if (type.IsEnum)
                {
                    foreach (var value in Enum.GetValues(type))
                    {
                        if (value.ToString().Equals("unknown", StringComparison.InvariantCultureIgnoreCase))
                            continue;
                        radioOptionContainer.InnerHtml.AppendHtml(AddRadioOption(AspFor.Name, value as Enum, AspFor.Model));
                    }
                }
                else
                {
                    // no values in model, can't display any options
                    if (AspFor.Model == null) 
                        return;

                    foreach (var value in AspFor.Model as IEnumerable)
                    {
                        radioOptionContainer.InnerHtml.AppendHtml(AddRadioOption(AspFor.Name, value, AspFor.Model));
                    }
                }

                container.InnerHtml.AppendHtml(radioOptionContainer);
                output.Content.AppendHtml(container);
                output.TagMode = TagMode.StartTagAndEndTag;
            }

            await base.ProcessAsync(context, output);
        }

        private TagBuilder AddTitle()
        {
            var builder = new TagBuilder("legend");
            builder.AddCssClass("govuk-fieldset__legend govuk-fieldset__legend--l");

            var h1Builder = new TagBuilder("h1");
            h1Builder.AddCssClass("govuk-fieldset__heading");
            h1Builder.InnerHtml.AppendHtml(Title);

            builder.InnerHtml.AppendHtml(h1Builder);

            return builder;
        }

        private TagBuilder AddRadioOption(string name, object value, object selected)
        {
            var builder = new TagBuilder("div");

            var id = $"{name.Replace(".", "-")}-{value}";

            builder.TagRenderMode = TagRenderMode.Normal;

            // get any existing classes
            builder.AddCssClass("govuk-radios__item");

            // add the id attribute
            object htmlAttributes = new
            {
                Id = id
            };

            // add the inner element - an input that takes the asp-for            
            var radioBtn = default(TagBuilder);
            var hiddenField = default(TagBuilder);

            radioBtn = _generator.GenerateRadioButton(
                ViewContext,
                AspFor?.ModelExplorer,
                name,
                value.ToString().ToLower(),
                null,
                htmlAttributes);
            radioBtn.AddCssClass("govuk-radios__input");

            // complete the tags for the radio option
            builder.InnerHtml.AppendHtml(radioBtn);

            if (hiddenField != null)
            {
                builder.InnerHtml.AppendHtml(hiddenField);
            }

            var childContent = new TagBuilder("label");
            childContent.AddCssClass("govuk-label govuk-radios__label");
            childContent.Attributes.Add("for", name);

            childContent.InnerHtml.AppendHtml(value.ToString());

            builder.InnerHtml.AppendHtml(childContent);

            return builder;
        }
    }
}
