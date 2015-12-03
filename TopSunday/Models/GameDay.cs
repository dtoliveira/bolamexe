using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("GameDay")]
    public class GameDay
    {
        GameDay()
        {
            Classification = new List<Classification>();
            Settings = new List<Settings>();
        }
        [Key]
        public int GameTypeID { get; set; }

        public string Description { get; set; }

        public virtual List<Classification> Classification { get; set; }
        public virtual List<Settings> Settings { get; set; }

    }
}