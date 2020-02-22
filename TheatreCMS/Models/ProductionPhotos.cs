using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheatreCMS.Models
{
    public class ProductionPhotos
    {
        [Key]
        public int ProPhotoId { get; set; }
        public byte[] Photo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [InverseProperty ("ProductionPhotos")]
        public virtual Production Production { get; set; }  

    }
}