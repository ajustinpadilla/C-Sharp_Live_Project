using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TheatreCMS.Models
{
    //Many the classes below are designed to be read by the helper method AdminSettingsReader().
    //Datatypes besides int/string or classes/lists of int/strings are liable to throw errors.
    //Any Classes/properties not meant to be used in Json string, is recommened to have [JsonIgnore] above,
    //or to place it in AdminSettings
    public class AdminSettings
    {
        [JsonProperty("season_productions")]
        public SeasonProductions season_productions { get; set; }

        public Footer FooterInfo { get; set; }

        [JsonProperty("models_missing_photos")]
        public ModelsMissingPhotos models_missing_photos { get; set; }

        [JsonProperty("recent_definition")]
        public RecentDefinition recent_definition { get; set; }

        [JsonProperty("on_stage")]
        public int on_stage { get; set; }

        [Required(ErrorMessage = "Please enter current season number")]     // for current season validation
        [JsonProperty("current_season")]
        public int current_season { get; set; }

        [JsonProperty("current_productions")]
        public List<int> current_productions { get; set; }     //a list of production IDs for current season
    }

    public class SeasonProductions
    {
        public int fall { get; set; }
        public int winter { get; set; }
        public int spring { get; set; }
    }
    public class RecentDefinition      //Lets Admin Define what they consider to be "recent", such as recent subscribers or productions
    {
        //// 0 = Date
        //// 1 = Span
        //public byte selection { get; set; }     // Span or Date
        public bool bUsingSpan { get; set; }    // If True, using Span instead of Date

        public int? span { get; set; }        //Number of months in the past

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }  // Earliest date for what is considered recent
    }

    public class Footer                  //All Information presented in Dashboard footer
    {
        public string AddressStreet { get; set; }
        public string AddressCityStateZip { get; set; }
        public string PhoneSales { get; set; }
        public string PhoneGeneral { get; set; }
    }
    public class ModelsMissingPhotos        //Used for methods reguarding missing photos in models
    {
        public List<int> productions { get; set; }
        public List<int> cast_members { get; set; }
        public List<int> sponsors { get; set; }
        public List<int> production_photos { get; set; }
    }
}

