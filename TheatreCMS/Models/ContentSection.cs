using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TheatreCMS.Models
{
    public class ContentSection
    {
        [Key]
        public int SectionId { get; set; }
        public ContentType ContentType { get; set; }
        public int ContentId { get; set; }
        public string CssId { get; set; }

    }
    public enum ContentType
    {
        Production,
        Sponsor,
        Info,
        Links
    }
}