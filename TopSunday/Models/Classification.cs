using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("Classification")]
    public class Classification
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("GameType")]
        public int GameTypeID { get; set; }
        public virtual GameType GameType { get; set; }

        [ForeignKey("Player")]
        public int PlayerID { get; set; }
        public virtual Player Player { get; set; }

        [ForeignKey("Settings")]
        public int SettingsId { get; set; }
        public virtual Settings Settings { get; set; }

        [ForeignKey("Season")]
        public int SeasonID { get; set; }
        public virtual Season Season { get; set; }


        public int NumGames { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Draws { get; set; }
        public int TotalPoints { get; set; }
        public int Goals { get; set; }
    }
}