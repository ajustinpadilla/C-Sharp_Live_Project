namespace TheatreCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Production
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Production()
        {
            Roles = new HashSet<Role>();
        }

        public int ProductionId { get; set; }

        public string Title { get; set; }

        public string Playwright { get; set; }

        public string Description { get; set; }

        public byte[] PromoPhoto { get; set; }

        public DateTime OpeningDay { get; set; }

        public DateTime ClosingDay { get; set; }

        public int Season { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
