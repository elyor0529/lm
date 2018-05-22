using System.Web.Mvc;
using LM.Api.Filters;

namespace LM.Api
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogErrorAttribute()); 
        }
    }
}
