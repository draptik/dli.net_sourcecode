using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Pipeline;
using Ploeh.Samples.CacheModel;

namespace Ploeh.Samples.Menu.StructureMap
{
    public partial class CacheLifecycle : ILifecycle
    {
        private readonly LeasedObjectCache cache;

        public CacheLifecycle(ILease lease)
        {
            if (lease == null)
            {
                throw new ArgumentNullException("lease");
            }

            this.cache = new LeasedObjectCache(lease);
        }

        public void EjectAll()
        {
            this.FindCache().DisposeAndClear();
        }

        public IObjectCache FindCache()
        {
            return this.cache;
        }

        public string Scope
        {
            get { return "Cache"; }
        }
    }

    public partial class CacheLifecycle
    {
        public ILease Lease
        {
            get { return this.cache.Lease; }
        }
    }
}
