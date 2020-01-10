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
        public byte[] PromoPhoto { get; set; }
        public DateTime OpeningDay { get; set; }
        public DateTime ClosingDay { get; set; }
        public int Season { get; set; }
        public virtual List<Role> Roles { get; set; }
    }
}