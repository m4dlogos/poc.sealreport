using System.Web;
using System.Web.Mvc;

namespace Telelogos.Reportings.WebServer
{
   public class FilterConfig
   {
      public static void RegisterGlobalFilters(GlobalFilterCollection filters)
      {
         filters.Add(new HandleErrorAttribute());
      }
   }
}
