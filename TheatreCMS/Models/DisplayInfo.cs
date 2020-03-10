using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheatreCMS.Models
{
    public class DisplayInfo
    {
        [Key]
        public int InfoId { get; set; }         // info primary key
        public string Title { get; set; }       // info title
        public string Description { get; set; } // info description
        public byte[] Image { get; set; }       // attached image
        public string File { get; set; }        // file path for attached file
    }
}