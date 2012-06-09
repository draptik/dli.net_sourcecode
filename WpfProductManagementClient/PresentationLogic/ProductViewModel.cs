using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public bool IsSelected { get; set; }

        public string Name { get; set; }

        public MoneyViewModel UnitPrice { get; set; }

        public ProductEditorViewModel Edit()
        {
            var editor = new ProductEditorViewModel(this.Id);
            editor.Currency = this.UnitPrice.CurrencyCode;
            editor.Name = this.Name;
            editor.Price = this.UnitPrice.Amount.ToString("F");
            return editor;
        }
    }
}
