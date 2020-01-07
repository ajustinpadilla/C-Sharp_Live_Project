namespace TheatreCMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Role
    {
        public int RoleID { get; set; }

        public string Character { get; set; }

        public int Type { get; set; }

        public string Details { get; set; }

        public int? Person_CastMemberID { get; set; }

        public int? Play_ProductionId { get; set; }

        public virtual CastMember CastMember { get; set; }

        public virtual Production Production { get; set; }
    }
}
