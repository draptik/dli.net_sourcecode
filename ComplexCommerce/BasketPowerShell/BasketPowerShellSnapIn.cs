using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.ComponentModel;

namespace Ploeh.Samples.Commerce.BasketPowerShell
{
    [RunInstaller(true)]
    public class BasketPowerShellSnapIn : PSSnapIn
    {
        public override string Description
        {
            get { return "Sample snap-in for the Dependency Injection in .NET sample commerce application offering basket management functionality."; }
        }

        public override string Name
        {
            get { return "SampleCommerceBasketSnapIn"; }
        }

        public override string Vendor
        {
            get { return "Ploeh"; }
        }
    }
}
