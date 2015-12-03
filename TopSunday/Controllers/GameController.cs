using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopSunday.Models;

namespace TopSunday.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Match()
        {
            List<Player> list = new List<Player>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                list = db.Player.ToList<Player>();
            }

            ViewBag.Players = list;

            return View();
        }
    }
}