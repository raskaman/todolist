using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
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

            InitDatabase();
        }

        void InitDatabase()
        {
            Database.SetInitializer<TodoListContext>(new DropCreateDatabaseIfModelChanges<TodoListContext>());

            using (var context = new TodoListContext())
            {
                if (context.Tasks.Count() == 0)
                {
                    context.Tasks.Add
                        (
                        new Task()
                        {
                            title = "Need to buy my milk",
                            lastUpdated = DateTime.Now,
                        }
                    );

                    context.Tasks.Add
                        (
                        new Task()
                        {
                            title = "Call my wife",
                            lastUpdated = DateTime.Now,
                        }
                    );
                }
            }
        }
    }
}
