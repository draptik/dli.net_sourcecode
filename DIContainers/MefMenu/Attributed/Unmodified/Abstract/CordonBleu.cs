﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract
{
    [Export(typeof(ICourse))]
    public partial class CordonBleu : ICourse { }

    public partial class CordonBleu : Ploeh.Samples.MenuModel.CordonBleu
    {
    }
}
