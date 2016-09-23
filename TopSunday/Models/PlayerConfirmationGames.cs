using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("PlayerConfirmationGames")]
    public class PlayerConfirmationGames
    {
        PlayerConfirmationGames()
        {

        }

        [Key]
        public int ID { get; set; }

        public bool GoToGame { get; set; }
        public DateTime GameDate { get; set; }

        [ForeignKey("Player")]
        public int PlayerID { get; set; }
        public virtual Player Player { get; set; }

        [ForeignKey("GameType")]
        public int GameTypeID { get; set; }
        public virtual GameType GameType { get; set; }

        [ForeignKey("Season")]
        public int SeasonID { get; set; }
        public virtual Season Season { get; set; }
    }
}