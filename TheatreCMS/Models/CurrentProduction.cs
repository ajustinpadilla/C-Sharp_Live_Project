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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Opening Day")]
        public DateTime OpeningDay { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Closing Day")]
        public DateTime ClosingDay { get; set; }

        public byte[] Image { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{hh: mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Evening Showtime")]
        public DateTime ShowtimeEve { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{hh: mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Matinee Showtime")]
        public DateTime ShowtimeMat { get; set; }
        [Display(Name = "Ticket Link")]
        public string TicketLink { get; set; }
        public virtual CalendarEvent CalendarEvent { get; set; }
    }
}
