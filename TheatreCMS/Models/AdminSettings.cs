using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace TheatreCMS.Models
{
	public class AdminSettings
	{
		public int current_season { get; set; }
		public seasonProductions season_productions { get; set; }
		public recentDefinition recent_definition { get; set; }
		public int onstage { get; set; }
		public class seasonProductions
		{
			public int fall { get; set; }
			public int winter { get; set; }
			public int spring { get; set; }
		}
		public class recentDefinition
		{
			public int span { get; set; }
			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
			public DateTime date { get; set; }
		}


        // This property will hold a production, selected by user
        [Required]
        [Display(Name = "Production")]
        public string Production { get; set; }
        // This property will hold all available productions for selection
        public IEnumerable<SelectListItem> Productions { get; set; }

    }
}
