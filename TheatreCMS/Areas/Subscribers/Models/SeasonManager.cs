using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.ApplicationServices;
using System.ComponentModel.DataAnnotations.Schema;
using TheatreCMS.Models;

namespace TheatreCMS.Areas.Subscribers.Models
{
    public class SeasonManager
    {
        [Key]
        public int NumberSeats { get; set; }
        public bool BookedCurrent { get; set; }
        public string FallProd { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{00:00}", ApplyFormatInEditMode = true)]
        public DateTime FallTime { get; set; }
        public bool BookedFall { get; set; }
        public string WinterProd { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{00:00}", ApplyFormatInEditMode = true)]
        public DateTime WinterTime { get; set; }
        public bool BookedWinter { get; set; }
        public string SpringProd { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{00:00}", ApplyFormatInEditMode = true)]
        public DateTime SpringTime { get; set; }
        public bool BookedSpring { get; set; }
        public virtual ApplicationUser Subscriber { get; set; }

    }
}