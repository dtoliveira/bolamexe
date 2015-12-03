using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    public class Settings
    {
        [Key]
        public int ID { get; set; }
        public int TablePosition { get; set; }

        [ForeignKey("GameDay")]
        public int GameTypeID { get; set; }
        public virtual GameDay GameDay { get; set; } 
    }
}