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

        //Play attribute needs help in the Create() action of the Role controller
        public virtual Production Production { get; set; }

        public string Character { get; set; }

        //Type attribute needs help in the Create() action of the Role controller
        public PositionEnum Type { get; set; }

        public virtual CastMember Person { get; set; }
        public string Details { get; set; }

    }
}