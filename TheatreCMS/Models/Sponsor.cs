using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TheatreCMS.Models
{
    public class Sponsor
    {
        [Key]
        public int SponsorId { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }

    }
} 