using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract
{
    [Export(typeof(ICourse))]
    public partial class CaesarSalad
    {
        [Import(AllowDefault = true)]
        public override IIngredient Extra
        {
            get { return base.Extra; }
            set
            {
                if (value == null)
                {
                    return;
                }
                base.Extra = value; 
            }
        }
    }

    public partial class CaesarSalad : Ploeh.Samples.MenuModel.CaesarSalad
    {
    }
}
