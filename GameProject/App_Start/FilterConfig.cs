using System.Web;
using System.Web.Mvc;
using GameProject.Filters;

namespace GameProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new GameSectionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
