using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EllosPratas.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
