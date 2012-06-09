using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.UpdateCurrency.CommandLine
{
    public class HelpCommand : ICommand
    {
        #region ICommand Members

        public void Execute()
        {
            Console.WriteLine("Usage: UpdateCurrency <DKK | EUR | USD> <DKK | EUR | USD> <rate>.");
        }

        #endregion
    }
}
