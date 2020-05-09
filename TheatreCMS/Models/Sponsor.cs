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
        public int SponsorId { get; set; }  // sponsor primary key
        public string Name { get; set; }    // sponsor name
        [Display(Name ="Logo Image")]       
        public int LogoId { get; set; }     // ID of photo.cs object
        public int? Height { get; set; }    // logo display height (may be different from original)
        public int? Width { get; set; }     // logo display width (may be different from original)
        public bool Current { get; set; }   // active sponsor
        public string Link { get; set; }        //url of sponsor website
    }
} 