using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("GameType")]
    public class GameType
    {
        public GameType()
        {
            Classification = new List<Classification>();
            GameTeams = new List<Models.GameTeams>();
            PlayerConfirmationGames = new List<Models.PlayerConfirmationGames>();
            CurrentGame = new List<Models.CurrentGame>();
        }

        [Key]
        public int GameTypeID { get; set; }
        public string Description { get; set; }

        public virtual List<Classification> Classification { get; set; }
        public virtual List<GameTeams> GameTeams { get; set; }
        public virtual List<PlayerConfirmationGames> PlayerConfirmationGames { get; set; }
        public virtual List<CurrentGame> CurrentGame { get; set; }

    }
}