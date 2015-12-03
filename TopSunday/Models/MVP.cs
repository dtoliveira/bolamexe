using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("MVP")]
    public class MVP
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Player")]
        public int PlayerID { get; set; }
        public virtual Player Player { get; set; }

        [ForeignKey("GameDay")]
        public int GameTypeID { get; set; }
        public virtual GameDay GameDay { get; set; }

        public DateTime Date { get; set; }


    }
}