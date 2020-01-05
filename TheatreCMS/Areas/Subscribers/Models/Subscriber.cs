﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheatreCMS.Models;

namespace TheatreCMS.Areas.Subscribers.Models
{
    public class Subscriber
    {
        public int SubscriberId { get; set; }
        public bool CurrentSubscriber { get; set; }
        public bool HasRenewed { get; set; }
        public bool Newsletter { get; set; }
        public bool RecentDonor { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastDonated { get; set; }
        public decimal LastDonationAmt { get; set; }
        public string SpecialRequests { get; set; }
        public string Notes { get; set; }
        public virtual ApplicationUser Subscribers { get; set; }
    }
}