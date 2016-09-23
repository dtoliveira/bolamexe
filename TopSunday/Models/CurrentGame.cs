using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TopSunday.Models
{
    [Table("CurrentGame")]
    public class CurrentGame
    {
        [Key]
        public int ID { get; set; }
        public DateTime GameDate { get; set; }
        public bool WasOpen { get; set; }

        [ForeignKey("GameType")]
        public int GameTypeID { get; set; }
        public virtual GameType GameType { get; set; }

       
    }
}