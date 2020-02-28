using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace TheatreCMS.Enum
{
    //Enum for the Wiki Schema
    public enum PositionEnum
    {
        //Cast member job position
        [Description("Actor")]
        Actor,
        [Description("Director")]
        Director,
        [Description("Technician")]
        Technician,
        [Description("Stage Manager")]
        StageManager,
        [Description("Other")]
        Other
    }
}