using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheatreCMS.Models;

namespace TheatreCMS.Areas.Subscribers.Models
{
    public class SeasonManager
    {
        [Key]
        [ForeignKey("SeasonManagerPerson")]
        public string SeasonManagerId { get; set; }
        public int NumberSeats { get; set; }
        public bool BookedCurrent { get; set; }
        public string FallProd { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? FallTime { get; set; }
        public bool BookedFall { get; set; }
        public string WinterProd { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? WinterTime { get; set; }
        public bool BookedWinter { get; set; }
        public string SpringProd { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? SpringTime { get; set; }
        public bool BookedSpring { get; set; }
        [Required]
        public virtual ApplicationUser SeasonManagerPerson { get; set; }

    }
}