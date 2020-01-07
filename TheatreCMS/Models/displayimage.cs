namespace TheatreCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("displayimage")]
    public partial class displayimage
    {
        [Key]
        public int InfoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public string File { get; set; }
    }
}
