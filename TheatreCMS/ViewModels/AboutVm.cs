using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheatreCMS.Models;

namespace TheatreCMS.ViewModels
{
    public class AboutVm
    {
        public IEnumerable<Award> Awards { get; set; }
        public IEnumerable<Sponsor> Sponsors { get; set; }
    }
}