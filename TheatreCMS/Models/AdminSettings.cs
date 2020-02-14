using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TheatreCMS.Models
{
	public class AdminSettings
	{
		public int current_season { get; set; }
		public int onstage { get; set; }
		public seasonProductions season_productions { get; set; }
		public recentDefinition recent_definition { get; set; }

		public class seasonProductions
		{
			public int fall { get; set; }
			public int winter { get; set; }
			public int spring { get; set; }
		}
		public class recentDefinition
		{
			public int span { get; set; }
			public DateTime date { get; set; }
		}
		
	}
}
 