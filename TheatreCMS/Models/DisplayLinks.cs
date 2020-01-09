using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheatreCMS.Models
{
    public class DisplayLinks
    {
        [Key]
        public int LinkId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
    }
}