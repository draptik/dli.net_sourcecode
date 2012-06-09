using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    internal class TestableValuableItem : IValuableItem<TestableValuableItem>, IEquatable<TestableValuableItem>
    {
        public override bool Equals(object obj)
        {
            var item = obj as TestableValuableItem;
            if (item != null)
            {
                return this.Equals(item);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        #region IValuableItem<TestableValuableItem> Members

        public Money Value { get; set; }

        public TestableValuableItem ConvertTo(Currency currency)
        {
            return new TestableValuableItem { Value = this.Value.ConvertTo(currency) };
        }

        #endregion

        #region IEquatable<TestableValuableItem> Members

        public bool Equals(TestableValuableItem other)
        {
            return this.Value.Equals(other.Value);
        }

        #endregion
    }
}
