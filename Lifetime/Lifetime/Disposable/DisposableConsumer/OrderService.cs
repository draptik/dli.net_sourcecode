using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime.Disposable.DisposableConsumer
{
    public class OrderService : IDisposable
    {
        private readonly OrderRepository repository;

        public OrderService(OrderRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        public OrderRepository Repository
        {
            get { return this.repository; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.repository.Dispose();
            }
        }
    }
}
