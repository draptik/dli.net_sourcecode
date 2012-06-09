using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Pipeline;
using Ploeh.Samples.CacheModel;

namespace Ploeh.Samples.Menu.StructureMap
{
    public partial class LeasedObjectCache : IObjectCache
    {
        private readonly IObjectCache objectCache;
        private readonly ILease lease;

        public LeasedObjectCache(ILease lease)
        {
            if (lease == null)
            {
                throw new ArgumentNullException("lease");
            }

            this.lease = lease;
            this.objectCache = new MainObjectCache();
        }

        public int Count
        {
            get
            {
                this.CheckLease();
                return this.objectCache.Count;
            }
        }

        public void DisposeAndClear()
        {
            this.objectCache.DisposeAndClear();
        }

        public void Eject(Type pluginType, Instance instance)
        {
            this.ObjectCache.Eject(pluginType, instance);
            this.CheckLease();
        }

        public bool Has(Type pluginType, Instance instance)
        {
            this.CheckLease();
            return this.objectCache.Has(pluginType, instance);
        }

        public object Locker
        {
            get { return this.objectCache.Locker; }
        }

        public object Get(Type pluginType, Instance instance)
        {
            this.CheckLease();
            return this.objectCache.Get(pluginType, instance);
        }

        public void Set(Type pluginType, Instance instance, object value)
        {
            this.objectCache.Set(pluginType, instance, value);
            this.lease.Renew();
        }

        private void CheckLease()
        {
            if (this.lease.IsExpired)
            {
                this.objectCache.DisposeAndClear();
            }
        }
    }

    public partial class LeasedObjectCache
    {
        public LeasedObjectCache(ILease lease, IObjectCache objectCache)
        {
            if (lease == null)
            {
                throw new ArgumentNullException("lease");
            }
            if (objectCache == null)
            {
                throw new ArgumentNullException("objectCache");
            }

            this.lease = lease;
            this.objectCache = objectCache;
        }

        public IObjectCache ObjectCache
        {
            get { return this.objectCache; }
        }

        public ILease Lease
        {
            get { return this.lease; }
        }
    }
}
