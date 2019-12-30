﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace TheatreCMS.Enum
{
    //Enum for the Wiki Schema
    public enum PositionEnum
    {
        [Description("Actor")]
        Actor,
        [Description("Director")]
        Director,
        [Description("Technician")]
        Technician,
        [Description("StageManager")]
        StageManager,
        [Description("Other")]
        Other
    }
}