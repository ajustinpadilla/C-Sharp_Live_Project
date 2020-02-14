using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Drawing;



namespace TheatreCMS.Models
{
    public class Production
    {
        [Key]
        public int ProductionId { get; set; }
        public string Title { get; set; }
        public string Playwright { get; set; }
        public string Description { get; set; }

        [Display(Name = "Opening Day")]
        public DateTime OpeningDay { get; set; }

        [Display(Name = "Closing Day")]
        public DateTime ClosingDay { get; set; }

        [Display(Name = "Promo Photo")]
        public byte[] PromoPhoto { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Evening Showtime")]
        public DateTime? ShowtimeEve { get; set; }
        
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Matinee Showtime")]
        public DateTime? ShowtimeMat { get; set; }

        [Display (Name = "Ticket Link")]
        public string TicketLink { get; set; }
        public int Season { get; set; }
        public bool IsCurrent { get; set; }
        public byte[] ShowDays { get; set; }
        public virtual ICollection<Part> Parts { get; set; }
        public virtual ICollection<CalendarEvent> Events { get; set; }
        public virtual ICollection<ProductionPhotos> ProductionPhotos { get; set; }
    }
}