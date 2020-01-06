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
        public int YearJoined { get; set; }
        public PositionEnum MainRole { get; set; }
        public string Bio { get; set; }
        public Image Photo { get; set; }
        public bool CurrentMember { get; set; }
        public virtual List<Role> Roles { get; set; }
    }
}