using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("Players_GameType")]
    public class Players_GameType
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Player")]
        public int PlayerID { get; set; }
        public virtual Player Player { get; set; }

        [ForeignKey("GameType")]
        public int GameTypeID { get; set; }
        public virtual GameType GameType { get; set; }

        public bool IsSubstitute { get; set; }
    }
}