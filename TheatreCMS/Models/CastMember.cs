namespace TheatreCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CastMember
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CastMember()
        {
            Roles = new HashSet<Role>();
        }

        public int CastMemberID { get; set; }

        public string Name { get; set; }

        public int YearJoined { get; set; }

        public int MainRole { get; set; }

        public string Bio { get; set; }

        //Photo attribute needs work in the Create() action of the CastMemembersController
        public byte[] Photo { get; set; }

        public bool CurrentMember { get; set; }

        //Roles attribute needs work in the Create() action of the CastMemembersController
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Roles { get; set; }

    }
}
