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
        Player()
        {
            Classification = new List<Classification>();
            MVP = new List<MVP>();
            GoldBidon = new List<GoldBidon>();
        }
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public decimal Debit { get; set; }

        public virtual List<Classification> Classification { get; set; }
        public virtual List<MVP> MVP { get; set; }
        public virtual List<GoldBidon> GoldBidon { get; set; }


    }
}