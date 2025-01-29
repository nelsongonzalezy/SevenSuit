using DbService;
using DbService.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SevenSuiteTest.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SevenSuiteTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserInterface _user;

        public HomeController(ILogger<HomeController> logger, IUserInterface user)
        {
            _logger = logger;
            _user = user;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Users model)
        {
            if (ModelState.IsValid)
            {
                var validacion = await _user.Authenticate(model.Mail, model.Password);
                bool isValidUser = validacion != null;

                if (isValidUser)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Mail)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Las credenciales son incorrectas." });
            }

            return Json(new { success = false, message = "Datos del formulario inválidos." });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Account");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
