namespace TheatreCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContentSection
    {
        [Key]
        public int SectionId { get; set; }

        public string ContentType { get; set; }

        public int ContentId { get; set; }

        public string CssId { get; set; }
    }
}
