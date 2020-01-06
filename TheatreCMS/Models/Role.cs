using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TheatreCMS.Enum;


namespace TheatreCMS.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public Production Play { get; set; }
        public string Character { get; set; }
        public PositionEnum Type { get; set; }
        public CastMember Person { get; set; }
        public string Details { get; set; }

    }
}