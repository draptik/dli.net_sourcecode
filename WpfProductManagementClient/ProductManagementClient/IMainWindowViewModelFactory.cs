using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;

namespace Ploeh.Samples.ProductManagement.WpfClient
{
    public interface IMainWindowViewModelFactory
    {
        MainWindowViewModel Create(IWindow window);
    }
}
