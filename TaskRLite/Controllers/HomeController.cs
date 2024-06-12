using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskRLite.Data;
using TaskRLite.Models;

namespace TaskRLite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TaskRContext _ctx;

        public HomeController(ILogger<HomeController> logger, TaskRContext ctx)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _ctx.AppUserRoles.Add(new() { RoleName = "SacklPicker" });
            _ctx.SaveChanges();
            var roles = _ctx.AppUserRoles.ToList();
            return View(roles);
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ctx.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
