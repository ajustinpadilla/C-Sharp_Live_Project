namespace TheatreCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CurrentProduction
    {
        [Key]
        public int ProductionId { get; set; }

        public string Title { get; set; }

        public string Playwright { get; set; }

        public DateTime OpeningDay { get; set; }

        public DateTime ClosingDay { get; set; }

        public byte[] Image { get; set; }

        public DateTime ShowtimeEve { get; set; }

        public DateTime ShowtimeMat { get; set; }

        public string TicketLink { get; set; }
        public virtual CalendarEvent CalendarEvent { get; set; }
    }
}
