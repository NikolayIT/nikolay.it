using System;
using System.Linq.Expressions;
using System.Web.Mvc;
namespace BlogSystem.Web.Helpers.Html
{
    //    <label>Name</label>
    //    <div class="row margin-bottom-20">
    //        <div class="col-md-7 col-md-offset-0">
    //            <input type="text" class="form-control">
    //        </div>
    //    </div>W
    public static class Bootstrap
    {
        public static MvcHtmlString BootstrapFormTextInputFor<TModel, TValue>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression,
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string propertyName = metaData.PropertyName;

            var label = GenerateLabel(propertyName);
            var outerDiv = GenerateOuterDiv();
            var innerDiv = GenerateInnerDiv();
            var input = GenerateTextBox(propertyName, htmlAttributes);

            innerDiv.InnerHtml = input.ToString();
            outerDiv.InnerHtml = innerDiv.ToString();

            return new MvcHtmlString(label.ToString() + outerDiv.ToString());
        }

        private static TagBuilder GenerateLabel(string name)
        {
            var label = new TagBuilder("label");
            label.SetInnerText(name);
            return label;
        }

        private static TagBuilder GenerateOuterDiv()
        {
            var outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("row margin-bottom-20");
            return outerDiv;
        }

        private static TagBuilder GenerateInnerDiv()
        {
            var innerDiv = new TagBuilder("div");
            innerDiv.AddCssClass("col-md-7 col-md-offset-0");
            return innerDiv;
        }

        private static TagBuilder GenerateTextBox(string name, object htmlAttributes)
        {
            var input = new TagBuilder("input");
            input.AddCssClass("form-control");
            input.Attributes.Add("type", "text");
            input.Attributes.Add("name", name);
            input.Attributes.Add("id", HtmlHelper.GenerateIdFromName(name));

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                input.MergeAttributes(attributes);
            }

            return input;
        }
    }
}
