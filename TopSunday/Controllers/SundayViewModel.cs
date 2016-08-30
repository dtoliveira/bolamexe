using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopSunday.Controllers
{
    public class SundayViewModel
    {
        public List<Models.Classification> Classification { get; set; }
        public string TeamA { get; internal set; }
        public string TeamB { get; internal set; }
    }
}
