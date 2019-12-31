using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheatreCMS.Models
{
    public class CurrentProduction
    {
        public int ProductionId { get; set; }
        public string Title { get; set; }
        public string Playwright { get; set; }
        public string Description { get; set; }
        //public image PromoPhoto { get; set; }
        public DateTime OpeningDay { get; set; }
        public DateTime ClosingDay { get; set; }
        public int Season { get; set; }
        public List<Role> { get; set; }
    }
}