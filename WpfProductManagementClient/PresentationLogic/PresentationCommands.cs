using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf
{
    public static class PresentationCommands
    {
        private readonly static RoutedCommand accept = new RoutedCommand("Accept", typeof(PresentationCommands));

        public static RoutedCommand Accept
        {
            get { return PresentationCommands.accept; }
        }
    }
}
