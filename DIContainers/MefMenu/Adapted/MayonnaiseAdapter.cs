using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Menu.Mef.Adapted
{
    public class MayonnaiseAdapter
    {
        private readonly Mayonnaise mayo;

        [ImportingConstructor]
        public MayonnaiseAdapter(
            EggYolk yolk, OliveOil oil)
        {
            if (yolk == null)
            {
                throw new ArgumentNullException("yolk");
            }
            if (oil == null)
            {
                throw new ArgumentNullException("oil");
            }

            this.mayo = new Mayonnaise(yolk, oil);
        }

        [Export]
        public virtual Mayonnaise Mayonnaise
        {
            get { return this.mayo; }
        }
    }
}
