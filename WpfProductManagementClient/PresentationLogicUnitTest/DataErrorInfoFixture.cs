using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf.UnitTest
{
    internal static class DataErrorInfoFixture
    {
        internal static string GetValidationResult<T, TProperty>(this Fixture fixture, Expression<Func<T, TProperty>> propertyPicker, TProperty value) where T : IDataErrorInfo
        {
            var p = (PropertyInfo)((MemberExpression)propertyPicker.Body).Member;
            var propertyName = p.Name;

            var sut = fixture.CreateAnonymous<T>();
            p.SetValue(sut, value, null);

            return sut[propertyName];
        }
    }
}
