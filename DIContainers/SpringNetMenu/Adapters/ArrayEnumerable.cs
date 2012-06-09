using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Menu.SpringNet.Adapters
{
    public partial class ArrayEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> sequence;

        public ArrayEnumerable(params T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            this.sequence = items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.sequence.GetEnumerator();
        }
    }

    public partial class ArrayEnumerable<T>
    {
        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
