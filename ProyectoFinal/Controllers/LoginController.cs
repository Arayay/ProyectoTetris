using System.Web.Mvc;
using ProyectoFinal.Core.Business;

namespace ProyectoFinal.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserBusiness _userBusiness;

        public LoginController()
        {
            _userBusiness = new UserBusiness();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = _userBusiness.ValidateUser(username, password);

            if (user == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }

            Session["UserId"] = user.UserId;
            Session["Username"] = user.Username;
            Session["Role"] = user.Role;

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password, string email)
        {
            var success = _userBusiness.RegisterUser(username, password, email);

            if (!success)
            {
                ViewBag.Error = "El usuario ya existe.";
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
