using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TheatreCMS.Models;

namespace TheatreCMS.Areas.Subscribers.Models
{
    public class Subscriber
    {
        [Key]
        [ForeignKey("SubscriberPerson")]
        public string SubscriberId { get; set; }        // subscriber primary key
        public bool CurrentSubscriber { get; set; }     // subscriber has purchased current season
        public bool HasRenewed { get; set; }            // subscriber has purchased next season (or simply creating new season manager would suffice)
        public bool Newsletter { get; set; }            // subscriber signed up for newsletter
        public bool RecentDonor { get; set; }           // subscriber donated "recently" as defined by the settings file
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastDonated { get; set; }      // date of last donation
        public decimal? LastDonationAmt { get; set; }   // amount of last donation
        public string SpecialRequests { get; set; }     // general special needs for bookings
        public string Notes { get; set; }               // general notes
        [Required]
        public virtual ApplicationUser SubscriberPerson { get; set; }   // associated user
    }
}