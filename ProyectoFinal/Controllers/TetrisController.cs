using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class TetrisController : Controller
    {
        // GET: Tetris
        public ActionResult Index()
        {
            return View();
        }
    }
}