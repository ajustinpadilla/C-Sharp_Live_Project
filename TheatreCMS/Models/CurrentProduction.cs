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
        public DateTime OpeningDay { get; set; }
        public DateTime ClosingDay { get; set; }
        public byte[] Image { get; set; }
        public DateTime ShowtimeEve { get; set; }
        public DateTime ShowtimeMat { get; set; }
        public string TicketLink { get; set; }

        public virtual ICollection<ContentSection> ContentSection { get; set; }
    }
}