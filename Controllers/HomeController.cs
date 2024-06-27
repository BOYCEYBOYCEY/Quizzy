using Microsoft.AspNetCore.Mvc;
using Quizzy_Main.Models;
using System.Diagnostics;

namespace Quizzy_Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        OnlineExamPortalContext db= new OnlineExamPortalContext();

        public IActionResult  ShowUser()
        {
            var count=db.Questions.Count();
            _logger.LogInformation("User showed from db");

            return View(db.Questions.ToList());
        }


       
       



        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
