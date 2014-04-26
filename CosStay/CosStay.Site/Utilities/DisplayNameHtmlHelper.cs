using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site
{
    public static class DisplayNameHtmlHelper
    {
        public static MvcHtmlString DisplayColumnNameFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper, TClass model, Expression<Func<TClass, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(TClass), name);
            var displayName = metadata.DisplayName;
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = name;
            return new MvcHtmlString(displayName);
        }

    }
}