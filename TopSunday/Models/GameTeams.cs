using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("GameTeams")]
    public class GameTeams
    {
        public GameTeams()
        {

        }

        [Key]
        public int ID { get; set; }

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

        public int Goals { get; set; }
        public string FinalResult { get; set; }
        //public int Points { get; internal set; }
    }
}