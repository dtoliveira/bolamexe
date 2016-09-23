using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("Season")]
    public class Season
    {
        Season()
        {

            Classification = new List<Models.Classification>();
            GameTeams = new List<Models.GameTeams>();
            PlayerConfirmationGames = new List<Models.PlayerConfirmationGames>();


        }
        [Key]
        public int ID { get; set; }
        public string SeasonDesc { get; set; }

        public virtual List<Classification> Classification { get; set; }
        public virtual List<GameTeams> GameTeams { get; set; }
        public virtual List<PlayerConfirmationGames> PlayerConfirmationGames { get; set; }

        

    }
}