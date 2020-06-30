using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheatreCMS.Models;

namespace TheatreCMS.ViewModels
{
    public class PhotoDependenciesVm
    {
        public List<ProductionPhotos> ProductionPhotos { get; set; }
        public List<Sponsor> Sponsors { get; set; }
        public bool HasDependencies { get; set; }
        public bool ValidId { get; set; }
    }
}