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
        public int SeasonManagerId { get; set; }    // season primary key
        public int NumberSeats { get; set; }        // number of seats available for book for each production
        public bool BookedCurrent { get; set; }     // 
        public string FallProd { get; set; }        // production name for fall
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? FallTime { get; set; }     // chosen date and time for fall production 
        public bool BookedFall { get; set; }        // fall booking approved
        public string WinterProd { get; set; }      // production name for winter
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? WinterTime { get; set; }   // chosen date and time for winter production 
        public bool BookedWinter { get; set; }      // winter booking approved
        public string SpringProd { get; set; }      // production name for spring
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? SpringTime { get; set; }   // chosen date and time for spring production 
        public bool BookedSpring { get; set; }      // spring booking approved
        [Required]
        public virtual ApplicationUser SeasonManagerPerson { get; set; }    // associated user

    }
}