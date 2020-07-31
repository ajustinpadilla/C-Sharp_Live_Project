using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheatreCMS.Models;

//Created for use in About.cshtml since the Sponsors model was already being referenced

namespace TheatreCMS.ViewModels
{
    public class AboutVm
    {
        public IEnumerable<Award> Awards { get; set; }
        public IEnumerable<Sponsor> Sponsors { get; set; }
    }
}