using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;

namespace Portal.Helpers.TagHelpers
{
    [HtmlTargetElement("radios", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RadioTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-title")]
        public string Title { get; set; }

        public ModelExpression? AspFor { get; set; }

        // need this if it's not an enum property we're mapping out
//        public ModelExpression? AspDataFor { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;

        public override int Order => 1;

        public RadioTagHelper(
            IHtmlGenerator generator) : base()
        {
            _generator = generator;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (AspFor == null)
                return;

            var dataProperty = AspFor;

//            if (AspDataFor != null)
//                dataProperty = AspDataFor;

//            var type = dataProperty.Metadata.ModelType;
//            var underlyingType = Nullable.GetUnderlyingType(type);

//            if (underlyingType != null)
//                type = underlyingType;

//            if (!type.IsEnum && type != typeof(bool) && type.GetInterface(nameof(IEnumerable)) == null)
//                return;

            output.TagName = "div";
            output.Attributes.Add("class", "govuk-form-group");

            var container = new TagBuilder("fieldset");
            container.AddCssClass("govuk-fieldset");
            container.InnerHtml.AppendHtml(AddTitle());

            var radioOptionContainer = new TagBuilder("div");
            radioOptionContainer.AddCssClass("govuk-radios");
            radioOptionContainer.Attributes.Add("data-module", "govuk-radios");

            // see if we have a required attribute for the property we're saving into
            var requiredAttribute = AspFor
                .Metadata?
                .ContainerType?
                .GetProperty(AspFor.Name)?
                .GetCustomAttributes(typeof(RequiredAttribute), false)
                .FirstOrDefault() as RequiredAttribute;

            var children = await output.GetChildContentAsync();
            string content = children.GetContent();
            container.InnerHtml.AppendHtml(content);

            container.InnerHtml.AppendHtml(radioOptionContainer);
            output.Content.AppendHtml(container);
            output.TagMode = TagMode.StartTagAndEndTag;

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
    }
}
