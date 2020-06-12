using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheatreCMS.Models
{
    public class Award
    {
        [Key]
        public int AwardId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public AwardType Type { get; set; }

        [Required]
        public string Category { get; set; }

        public string Recipient { get; set; }

        public int ProductionId { get; set; }
        public virtual Production Production { get; set; }

        public int CastMemberId { get; set; }
        public virtual CastMember CastMember { get; set; }

        public string OtherInfo { get; set; }

    }

    public enum AwardType
    {
        Award,
        Finalist,
    }
}   