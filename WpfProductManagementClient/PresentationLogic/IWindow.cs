using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf
{
    public interface IWindow
    {
        void Close();

        IWindow CreateChild(object viewModel);

        void Show();

        bool? ShowDialog();
    }
}
