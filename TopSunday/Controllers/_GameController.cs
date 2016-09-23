using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopSunday.Models;

namespace TopSunday.Controllers
{
    public class _GameController : Controller
    {
        public ActionResult HomePage()
        {
            return View();
        }

        public ActionResult WednesdayGames()
        {
            
            return View();
        }

        public ActionResult SundayGames()
        {
            return View();
        }


        // GET: Game
        public ActionResult Match()
        {

            try
            {


                List<Player> playersList = new List<Player>();

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    playersList = db.Player.ToList<Player>();
                }

                ViewBag.Players = playersList;
            }

            catch (Exception)
            {

                throw;
            }

            return View();
        }

        //public ActionResult SundayStats()
        //{
        //    GamesViewModel vm = new GamesViewModel();

        //    List<Classification> listSunday;
        //    List<Classification> listWednesday;

        //    ApplicationDbContext db = new ApplicationDbContext();


        //    listSunday = db.Classification.Where(x => x.GameType.Description.Equals("Sunday")).ToList<Classification>();
        //    listWednesday = db.Classification.Where(x => x.GameType.Description.Equals("Wednesday")).ToList<Classification>();




        //    ViewBag.SundayStats = listSunday;

        //    vm.Classification = listSunday;

        //    string principalURL_teamA = "http://lineupbuilder.com/2014/custom/354x526.php?p=5&amp;a=1&amp;t=Equipa%20A&amp;c=dc0000&amp;";
        //    string principalURL_teamB = "http://lineupbuilder.com/2014/custom/354x526.php?p=5&amp;a=1&amp;t=Equipa%20B&amp;c=0f00db&amp;";

        //    string formatTeamA = "1=GK_{0}%201_1_388_174&amp;2=ML_{1}%207_7_189_64&amp;3=DM_{2}%2010_10_267_180&amp;4=FC_{3}%209_9_75_173&amp;5=MR_{4}%2020_20_175_296&amp;6=MLA___204_64&amp;7=MCL___222_138&amp;8=MCR___222_211&amp;9=MRA___204_284&amp;10=FCL___98_138&amp;11=FCR___98_211&amp;c2=ffffff&amp;c3=ffffff&amp;output=embed";
        //    vm.TeamA = string.Format(principalURL_teamA + formatTeamA, listSunday[0].Player.Name, listSunday[3].Player.Name, listSunday[5].Player.Name, listSunday[6].Player.Name, listSunday[9].Player.Name);

        //    string formatTeamB = "1=GK_{0}t%201_1_388_174&amp;2=ML_{1}%207_7_189_64&amp;3=DM_{2}%204_4_267_180&amp;4=FC_{3}%2010_10_75_173&amp;5=MR_{4}%206_6_175_296&amp;6=MLA___204_64&amp;7=MCL___222_138&amp;8=MCR___222_211&amp;9=MRA___204_284&amp;10=FCL___98_138&amp;11=FCR___98_211&amp;c2=ffffff&amp;c3=ffffff&amp;output=embed";
        //    vm.TeamB = string.Format(principalURL_teamB + formatTeamB, listSunday[1].Player.Name, listSunday[2].Player.Name, listSunday[4].Player.Name, listSunday[7].Player.Name, listSunday[8].Player.Name);


        //    return View(vm);
        //}

        //public ActionResult WednesdayStats()
        //{
        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        ViewBag.WednesdayStats = db.Classification.Where(x => x.GameType.Description.Equals("Wednesday"));

        //    }

        //    return View(ViewBag.WednesdayStats);
        //}
    }
}