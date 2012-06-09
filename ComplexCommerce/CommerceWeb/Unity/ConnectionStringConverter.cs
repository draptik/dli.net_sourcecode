using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Globalization;
using System.Configuration;

namespace Ploeh.Samples.Commerce.Web.Unity
{
    public class ConnectionStringConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return ConfigurationManager.ConnectionStrings[value.ToString()].ConnectionString;
        }
    }
}