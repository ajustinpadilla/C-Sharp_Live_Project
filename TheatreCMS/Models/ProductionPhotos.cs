using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheatreCMS.Models
{
    public class ProductionPhotos
    {
        [Key]
        public int ProPhotoId { get; set; }
        public virtual Production Production { get; set; }
        public byte[] Photo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
    }
}