using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Graph;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using Ploeh.Samples.Commerce.Data.Sql;

namespace Ploeh.Samples.Commerce.Web.StructureMap
{
    public class ConnectionStringConvention : IRegistrationConvention
    {
        private readonly string connectionString;

        public ConnectionStringConvention(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            this.connectionString = connectionString;
        }

        #region IRegistrationConvention Members

        public void Process(Type type, Registry registry)
        {
            if (!type.Name.StartsWith("Sql"))
            {
                return;
            }
            if (type == typeof(SqlCurrency))
            {
                return;
            }

            IConfiguredInstance ctor = new ConstructorInstance(type);
            ctor.SetValue("connString", this.connectionString);
            registry.For(type.BaseType).HttpContextScoped().Add(ctor);
        }

        #endregion
    }
}
