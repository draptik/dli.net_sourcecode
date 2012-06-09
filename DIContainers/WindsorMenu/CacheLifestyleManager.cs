using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel;
using Ploeh.Samples.CacheModel;
using Castle.MicroKernel.Context;

namespace Ploeh.Samples.Menu.Windsor
{
    public partial class CacheLifestyleManager : 
        AbstractLifestyleManager
    {
        private ILease lease;

        public ILease Lease
        {
            get 
            {
                if (this.lease == null)
                {
                    this.lease = this.ResolveLease();
                }
                return this.lease;
            }
        }

        private ILease ResolveLease()
        {
            var defaultLease = new SlidingLease(TimeSpan.FromMinutes(1));
            if (this.Kernel == null)
            {
                return defaultLease;
            }
            if (this.Kernel.HasComponent(typeof(ILease)))
            {
                return this.Kernel.Resolve<ILease>();
            }
            return defaultLease;
        }
    }

    public partial class CacheLifestyleManager
    {
        public override bool Release(object instance)
        {
            return false;
        }

        private object obj;

        public override object Resolve(CreationContext context)
        {
            if (this.Lease.IsExpired)
            {
                base.Release(this.obj);
                this.obj = null;
            }
            if (this.obj == null)
            {
                this.Lease.Renew();
                this.obj = base.Resolve(context);
            }
            return this.obj;
        }

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if ((disposing)
                && this.obj != null)
            {
                this.Release(this.obj);
            }
        }
    }
}
