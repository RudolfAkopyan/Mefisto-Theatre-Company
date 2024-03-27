using System.Web;
using System.Web.Mvc;

namespace Mefisto_Theatre_Company
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
