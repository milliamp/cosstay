using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace CosStay.Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            /*DependencyResolver.SetResolver(type =>
            {
                if (type == typeof (IAsyncActionInvoker))
                    return new AsyncActionInvoker();
                return null;
            }, type => Enumerable.Empty<object>());*/
        }
    }
}
