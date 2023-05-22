using EvolvPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EvolvPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        

        public IActionResult Index()
        {
            EvolvProContext evolv = new EvolvProContext();
            evolv.SaveChangesAndNotify();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Menu()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contra)
        {
            EvolvProContext _DbContext = new EvolvProContext();
            var buscarNombre = _DbContext.Usuarios.FirstOrDefault(x => x.CorreoUsu == correo);


            if (buscarNombre != null)
            {
                if (buscarNombre != null && buscarNombre.ContrasenaUsu == contra)
                {
                    string nivel = buscarNombre.FkTipousu.ToString();
                    //TempData["NivelUsu"] = nivel; 
                    HttpContext.Session.SetString("VarSesion1", nivel);
                    return RedirectToAction("Menu", "Home");

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["Resultado"] = "Intente de nuevo";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}