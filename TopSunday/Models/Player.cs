using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("Player")]
    public class Player
    {
        public Player()
        {
            Classification = new List<Classification>();
            GameTeams = new List<Models.GameTeams>();
            PlayerConfirmationGames = new List<Models.PlayerConfirmationGames>();
        }

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Debit { get; set; }

        public virtual List<Classification> Classification { get; set; }
        public virtual List<GameTeams> GameTeams { get; set; }
        public virtual List<PlayerConfirmationGames> PlayerConfirmationGames { get; set; }

        



    }
}