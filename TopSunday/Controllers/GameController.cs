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

        public ActionResult SundayStats()
        {
            SundayViewModel vm = new SundayViewModel();

            List<Classification> list;
            ApplicationDbContext db = new ApplicationDbContext();


            list = db.Classification.Where(x => x.GameDay.Description.Equals("Sunday")).ToList<Classification>();



            ViewBag.SundayStats = list;
            vm.Classification = list;
            return View(vm);
        }

        public ActionResult WednesdayStats()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ViewBag.WednesdayStats = db.Classification.Where(x => x.GameDay.Description.Equals("Wednesday"));

            }

            return View(ViewBag.WednesdayStats);
        }
    }
}