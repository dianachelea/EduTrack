using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class AssignmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
