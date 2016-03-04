using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using TodoList.Models;

namespace TodoList
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
#if (DEBUG)
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TodoListContext>());
#endif
        }
    }
}
