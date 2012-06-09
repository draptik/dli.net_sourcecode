using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.CacheModel;
using Microsoft.Practices.Unity;

namespace Ploeh.Samples.Menu.Unity
{
    public partial class CacheLifetimeManager :
        LifetimeManager, IDisposable
    {
        private object value;
        private readonly ILease lease;

        public CacheLifetimeManager(ILease lease)
        {
            if (lease == null)
            {
                throw new ArgumentNullException("lease");
            }

            this.lease = lease;
        }

        public override object GetValue()
        {
            this.RemoveValue();
            return this.value;
        }

        public override void RemoveValue()
        {
            if (this.lease.IsExpired)
            {
                this.Dispose();
            }
        }

        public override void SetValue(object newValue)
        {
            this.value = newValue;
            this.lease.Renew();
        }
    }

    public partial class CacheLifetimeManager
    {
        public ILease Lease
        {
            get { return this.lease; }
        }

        public CacheLifetimeManager Clone()
        {
            return new CacheLifetimeManager(this.Lease);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var d = this.value as IDisposable;
                if (d != null)
                {
                    d.Dispose();
                }
                this.value = null;
            }
        }
    }
}
