using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TheatreCMS.Models
{
    public class AdminSettings
    {
        public static DateTime Now { get; }  //Add for current season auto calculation
        [Required(ErrorMessage = "Please enter current season number")]         // for current season validation
        public int current_season { get; set; }     // the theater season number for the current season
        public seasonProductions season_productions { get; set; }   // holds 3 production ID's for current season

        public List<int> current_productions { get; set; }  //a list of production IDs for current season

        public recentDefinition recent_definition { get; set; }     // holds recent span and date
        public int onstage { get; set; }            // production ID of current production

        public class seasonProductions
        {
            public int fall { get; set; }       // fall production ID for current season
            public int winter { get; set; }     // winter production ID for current season
            public int spring { get; set; }     // spring production ID for current season
        }

        public class recentDefinition
        {
            public int span { get; set; }       // number of months in the past
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime date { get; set; }  // earliest date for what is considered recent
        }

    }
}
