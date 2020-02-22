using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TheatreCMS.Models;

namespace TheatreCMS.Models
{
    public class CalendarEvent
    {
        [Key]
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? TicketsAvailable { get; set; } 
        public string Color { get; set; }
        public string ClassName { get; set; }
        public string SomeKey { get; set; }
        public bool AllDay { get; set; }

        public int? ProductionId { get; set; }

        public int? RentalRequestId { get; set; }
    }


}