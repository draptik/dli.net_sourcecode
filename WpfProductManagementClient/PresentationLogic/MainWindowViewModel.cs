using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf
{
    public class MainWindowViewModel
    {
        private readonly IProductManagementAgent agent;
        private readonly IWindow window;
        private readonly ObservableCollection<ProductViewModel> products;
        private readonly RelayCommand closeCommand;
        private readonly RelayCommand deleteProductCommand;
        private readonly RelayCommand editProductCommand;
        private readonly RelayCommand insertProductCommand;
        private readonly RelayCommand refreshCommand;

        public MainWindowViewModel(IProductManagementAgent agent, IWindow window)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("agent");
            }
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }        

            this.agent = agent;
            this.window = window;
            this.products = new ObservableCollection<ProductViewModel>();
            this.closeCommand = new RelayCommand(this.Close);
            this.deleteProductCommand = new RelayCommand(this.DeleteProduct, this.IsProductSelected);
            this.editProductCommand = new RelayCommand(this.EditProduct, this.IsProductSelected);
            this.insertProductCommand = new RelayCommand(this.InsertProduct);
            this.refreshCommand = new RelayCommand(this.Refresh);
        }

        public ICommand CloseCommand
        {
            get { return this.closeCommand; }
        }

        public ICommand DeleteProductCommand
        {
            get { return this.deleteProductCommand; }
        }

        public ICommand EditProductCommand
        {
            get { return this.editProductCommand; }
        }

        public ICommand InsertProductCommand
        {
            get { return this.insertProductCommand; }
        }

        public ObservableCollection<ProductViewModel> Products
        {
            get { return this.products; }
        }

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        private void Close(object parameter)
        {
            this.window.Close();
        }

        private void DeleteProduct(object parameter)
        {
            var product = this.Products.Single(p => p.IsSelected);
            var productId = product.Id;
            this.agent.DeleteProduct(productId);
            this.Refresh(new object());
        }

        private bool IsProductSelected(object parameter)
        {
            return this.Products.Where(p => p.IsSelected).Count() == 1;
        }

        private void EditProduct(object parameter)
        {
            var product = this.Products.Single(p => p.IsSelected);
            var editor = product.Edit();
            editor.Title = "Edit Product";

            if (this.window.CreateChild(editor).ShowDialog() ?? false)
            {
                this.agent.UpdateProduct(editor);
                this.Refresh(new object());
            }
        }

        private void InsertProduct(object parameter)
        {
            var editor = new ProductEditorViewModel(0);
            editor.Title = "Add Product";
            editor.Currency = "DKK";

            if (this.window.CreateChild(editor).ShowDialog() ?? false)
            {
                this.agent.InsertProduct(editor);
                this.Refresh(new object());
            }
        }

        private void Refresh(object parameter)
        {
            this.products.Clear();

            foreach (var p in this.agent.SelectAllProducts())
            {
                this.products.Add(p);
            }
        }
    }
}
