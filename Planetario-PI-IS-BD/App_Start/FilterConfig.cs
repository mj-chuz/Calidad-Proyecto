using System.Web;
using System.Web.Mvc;

namespace Planetario_PI_IS_BD
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
