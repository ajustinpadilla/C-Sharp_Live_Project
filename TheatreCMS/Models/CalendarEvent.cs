﻿using System;
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
        public int EventId { get; set; }            // event primary key
        public string Title { get; set; }           // event title
        public DateTime StartDate { get; set; }     // event start date and time
        public DateTime EndDate { get; set; }       // event end date and time
        public int? TicketsAvailable { get; set; }  // number of tickets remaining
        public string Color { get; set; }           // event color when rendered in full calendar
        public string ClassName { get; set; }       // 
        public string SomeKey { get; set; }         // 
        public bool AllDay { get; set; }            // all day event

        public int? ProductionId { get; set; }      // Id for associated production

        public int? RentalRequestId { get; set; }   // Id for associated rental request
    }


}