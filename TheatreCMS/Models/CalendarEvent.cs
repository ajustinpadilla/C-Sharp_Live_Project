using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual CurrentProduction Production { get; set; }
        public string Rental { get; set; }
    }


}