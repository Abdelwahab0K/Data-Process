using Microsoft.AspNetCore.Mvc;
using LandingPage.Models;

namespace LandingPage.Controllers
{
    public class TrialController : Controller
    {
        public IActionResult Index(string TypeFiltre, int? PceId, string Jour)
        {
            //if (string.IsNullOrEmpty(TypeFiltre)) TypeFiltre = "pce_id";
            //if (string.IsNullOrEmpty(Jour)) Jour = DateTime.Now.ToString("yyyy-MM-dd");
            //if (PceId == null) PceId = 0;

            //ViewBags
            ViewBag.TypeFiltre = TypeFiltre;
            ViewBag.PceId = PceId;
            ViewBag.Jour = Jour;

            return View();
        }
    }
}

