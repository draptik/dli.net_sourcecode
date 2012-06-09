using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Moq;

namespace Ploeh.Samples.Commerce.UpdateCurrency.CommandLine.UnitTest
{
    internal class CurrencyFixture : Fixture
    {
        internal CurrencyFixture()
        {
            this.Resolver = t =>
                {
                    var factoryType = typeof(MoqFactory<>).MakeGenericType(t);
                    var factory = (IMoqFactory)Activator.CreateInstance(factoryType, this);
                    return factory.Create().Object;
                };
        }

        internal Mock<T> CreateMoq<T>() where T : class
        {
            return new MoqFactory<T>(this).Create();
        }

        internal Mock<T> FreezeMoq<T>() where T : class
        {
            var td = this.CreateMoq<T>();
            this.Register(td.Object);
            return td;
        }

        private class MoqFactory<T> : IMoqFactory where T : class
        {
            private readonly Fixture fixture;

            public MoqFactory(Fixture fixture)
            {
                if (fixture == null)
                {
                    throw new ArgumentNullException("fixture");
                }

                this.fixture = fixture;
            }

            internal Mock<T> Create()
            {
                var td = new Mock<T>();
                td.CallBase = true;
                td.DefaultValue = DefaultValue.Mock;
                return td;
            }

            #region IMoqFactory Members

            Mock IMoqFactory.Create()
            {
                return this.Create();
            }

            #endregion
        }

        private interface IMoqFactory
        {
            Mock Create();
        }
    }
}
