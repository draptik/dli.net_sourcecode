using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public class JunkFood : IMeal
    {
        private readonly string name;

        internal JunkFood(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
