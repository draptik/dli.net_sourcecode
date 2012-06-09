using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;
using System.Threading;
using Domain = Ploeh.Samples.Commerce.Domain;
using System.Security.Principal;

namespace Ploeh.Samples.Commerce.Data.Sql
{
    public class SqlAuditor : IAuditor
    {
        private readonly CommerceObjectContext context;

        public SqlAuditor(string connString)
        {
            if (connString == null)
            {
                throw new ArgumentNullException("connString");
            }

            this.context = new CommerceObjectContext(connString);
        }

        #region IAuditor Members

        public void Record(Domain.AuditEvent @event)
        {
            switch (@event.Name)
            {
                case "ProductDeleted":
                    var id = (int)@event.Data;
                    this.ProductDeleted(@event.Timestamp, @event.Identity, id);
                    break;
                case "ProductInserted":
                    var inserted = (Domain.Product)@event.Data;
                    this.ProductInserted(@event.Timestamp, @event.Identity, inserted.Name, inserted.UnitPrice);
                    break;
                case "ProductUpdated":
                    var updated = (Domain.Product)@event.Data;
                    this.ProductUpdated(@event.Timestamp, @event.Identity, updated.Id, updated.Name, updated.UnitPrice);
                    break;
                default:
                    throw new ArgumentException("Unknown audit event.", "@event");
            }
        }

        #endregion

        private SqlCurrency CreateBaseCurrency()
        {
            var danishCurrency = new SqlCurrency("DKK", this.context.Connection.ConnectionString);
            return danishCurrency;
        }

        private void ProductDeleted(DateTimeOffset timestamp, IIdentity identity, int id)
        {
            var ae = new AuditEvent();
            ae.User = identity.Name;
            ae.Time = timestamp.UtcDateTime;
            ae.AuditProductsDeleted.Add(new AuditProductDeleted { ProductId = id });

            this.context.AddToAuditEvents(ae);
            this.context.SaveChanges();
        }

        private void ProductInserted(DateTimeOffset timestamp, IIdentity identity, string name, Money unitPrice)
        {
            var baseCurrency = this.CreateBaseCurrency();

            var ae = new AuditEvent();
            ae.User = identity.Name;
            ae.Time = timestamp.UtcDateTime;
            ae.AuditProductsInserted.Add(new AuditProductInserted { Name = name, UnitPrice = unitPrice.ConvertTo(baseCurrency).Amount });

            this.context.AddToAuditEvents(ae);
            this.context.SaveChanges();
        }

        private void ProductUpdated(DateTimeOffset timestamp, IIdentity identity, int id, string name, Money unitPrice)
        {
            var baseCurrency = this.CreateBaseCurrency();

            var ae = new AuditEvent();
            ae.User = identity.Name;
            ae.Time = timestamp.UtcDateTime;
            ae.AuditProductsUpdated.Add(new AuditProductUpdated { ProductId = id, Name = name, UnitPrice = unitPrice.ConvertTo(baseCurrency).Amount });

            this.context.AddToAuditEvents(ae);
            this.context.SaveChanges();
        }
    }
}
