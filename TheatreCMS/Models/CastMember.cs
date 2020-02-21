using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TheatreCMS.Enum;
using System.Drawing;


namespace TheatreCMS.Models
{
    public class CastMember
    {
        [Key]
        public int CastMemberID { get; set; }
        public string Name { get; set; }
        public Nullable<int> YearJoined { get; set; }
        public PositionEnum MainRole { get; set; }
        public string Bio { get; set; }

        //Photo attribute needs work in the Create() action of the CastMemembersController
        public byte[] Photo { get; set; }

        public bool CurrentMember { get; set; }

        //Parts attribute needs work in the Create() action of the CastMemembersController
        public virtual List<Part> Parts { get; set; }

        /* Need to find a way to explicitly match a CastMember's User account to their ApplicationUser object, 
        If a castmember signs up for an account, ensure that for ApplicationUser user "=" CastMember castMember,
        user.CastMemberPersonID = castMembe.CastMemberPersonID */
        //public virtual ApplicationUser CastMemberPerson { get; set; } 
        public string CastMemberPersonID { get; set; }
        public Nullable<bool> AssociateArtist { get; set;}
        public Nullable<bool> EnsembleMember { get; set; }
        public Nullable<int> CastYearLeft { get; set; }
        public Nullable<int> DebutYear { get; set; }
    }
}