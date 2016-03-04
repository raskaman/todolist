using System.Linq;
using System.Web.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (var context = new TodoListContext())
            {
                var users = context.Users.ToList();
                return View(users);
            }
        }

        public ActionResult Users()
        {
            return View();
        }
    }
}