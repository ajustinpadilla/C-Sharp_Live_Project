using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace TheatreCMS.Models
{
    public class CurrentProduction
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

        //public virtual ICollection<ContentSection> ContentSection { get; set; }
    }
}