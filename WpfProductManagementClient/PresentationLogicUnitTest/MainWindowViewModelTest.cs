using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.SemanticComparison.Fluent;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf.UnitTest
{
    public class MainWindowViewModelTest
    {
        [Theory, AutoMoqData]
        public void CreateWithNullAgentWillThrow(IWindow dummyDialogService)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MainWindowViewModel(null, dummyDialogService));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void CreateWithNullDialogServiceWillThrow(IProductManagementAgent dummyAgent)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new MainWindowViewModel(dummyAgent, null));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ProductsIsInstance(MainWindowViewModel sut)
        {
            // Fixture setup
            // Exercise system
            ObservableCollection<ProductViewModel> result = sut.Products;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void CloseCommandIsInstance(MainWindowViewModel sut)
        {
            // Fixture setup
            // Exercise system
            ICommand result = sut.CloseCommand;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ExecuteCloseCommandWillCloseWindow([Frozen]Mock<IWindow> windowMock, MainWindowViewModel sut, object p)
        {
            // Fixture setup
            // Exercise system
            sut.CloseCommand.Execute(p);
            // Verify outcome
            windowMock.Verify(w => w.Close());
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RefreshCommandIsInstance(MainWindowViewModel sut)
        {
            // Fixture setup
            // Exercise system
            ICommand result = sut.RefreshCommand;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ExecuteRefreshCommandWillCorrectlyPopulateProducts()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();
            var products = fixture.CreateMany<ProductViewModel>().ToList();

            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.SelectAllProducts()).Returns(products);

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            fixture.AddManyTo(sut.Products);
            // Exercise system
            fixture.Do((object p) => sut.RefreshCommand.Execute(p));
            // Verify outcome
            Assert.True(products.SequenceEqual(sut.Products), "RefreshCommand");
            // Teardown
        }

        [Theory, AutoMoqData]
        public void InsertProductCommandIsInstance(MainWindowViewModel sut)
        {
            // Fixture setup
            // Exercise system
            ICommand result = sut.InsertProductCommand;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ExecuteInsertProductCommandWillShowCorrectDialog(Mock<IWindow> childWindowMock, [Frozen]Mock<IWindow> windowStub, MainWindowViewModel sut, object p)
        {
            // Fixture setup
            var expectedVM = new ProductEditorViewModel(0)
            {
                Currency = "DKK",
                Name = string.Empty,
                Price = string.Empty,
                Title = "Add Product"
            }.AsSource().OfLikeness<ProductEditorViewModel>();

            windowStub.Setup(w => w.CreateChild(expectedVM))
                .Returns(childWindowMock.Object);
            // Exercise system
            sut.InsertProductCommand.Execute(p);
            // Verify outcome
            childWindowMock.Verify(w => w.ShowDialog());
            // Teardown
        }

        [Fact]
        public void ExecuteInsertProductCommandWillNotInsertIntoAgentWhenReturnValueIsNull()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();

            fixture.Freeze<Mock<IWindow>>().Setup(w => w.CreateChild(It.IsAny<object>())).Returns(() =>
                {
                    var childStub = fixture.CreateAnonymous<Mock<IWindow>>();
                    childStub.Setup(cw => cw.ShowDialog()).Returns((bool?)null);
                    return childStub.Object;
                });

            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            // Exercise system
            fixture.Do((object p) => sut.InsertProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.InsertProduct(It.IsAny<ProductEditorViewModel>()), Times.Never());
            // Teardown
        }

        [Fact]
        public void ExecuteInsertProductCommandWillNotInsertIntoAgentWhenReturnValueIsFalse()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();

            fixture.Freeze<Mock<IWindow>>().Setup(w => w.CreateChild(It.IsAny<object>())).Returns(() =>
            {
                var childStub = fixture.CreateAnonymous<Mock<IWindow>>();
                childStub.Setup(cw => cw.ShowDialog()).Returns(false);
                return childStub.Object;
            });

            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            // Exercise system
            fixture.Do((object p) => sut.InsertProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.InsertProduct(It.IsAny<ProductEditorViewModel>()), Times.Never());
            // Teardown
        }

        [Fact]
        public void ExecuteInsertProductCommandWillInsertIntoAgentWhenReturnValueIsTrue()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();

            var product = fixture.CreateAnonymous<ProductEditorViewModel>().AsSource().OfLikeness<ProductEditorViewModel>()
                .Without(d => d.Title)
                .Without(d => d.Error)
                .Without(d => d.IsValid)
                .Without(d => d.Id);

            fixture.Freeze<Mock<IWindow>>()
                .Setup(w => w.CreateChild(It.IsAny<object>()))
                .Callback((object vm) =>
                    {
                        var pVM = (ProductEditorViewModel)vm;
                        pVM.Currency = product.Value.Currency;
                        pVM.Name = product.Value.Name;
                        pVM.Price = product.Value.Price;
                    })
                .Returns(() =>
                    {
                        var childStub = fixture.CreateAnonymous<Mock<IWindow>>();
                        childStub.Setup(cw => cw.ShowDialog()).Returns(true);
                        return childStub.Object;
                    });

            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            // Exercise system
            fixture.Do((object p) => sut.InsertProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.InsertProduct(It.Is<ProductEditorViewModel>(pvm => product.Equals(pvm))));
            // Teardown
        }

        [Fact]
        public void ExecuteInsertProductCommandWillReloadProducts()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();

            fixture.Freeze<Mock<IWindow>>().Setup(w => w.CreateChild(It.IsAny<object>())).Returns(() =>
                {
                    var childStub = fixture.CreateAnonymous<Mock<IWindow>>();
                    childStub.Setup(cw => cw.ShowDialog()).Returns(true);
                    return childStub.Object;
                });
            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            // Exercise system
            fixture.Do((object p) => sut.InsertProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.SelectAllProducts());
            // Teardown
        }

        [Theory, AutoMoqData]
        public void EditProductCommandIsInstance(MainWindowViewModel sut)
        {
            // Fixture setup
            // Exercise system
            ICommand result = sut.EditProductCommand;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void EditProductCommandCanExecuteWhenProductIsSelected()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();
            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            sut.Products.Add(fixture.Build<ProductViewModel>()
                .With(x => x.IsSelected, true)
                .CreateAnonymous());
            // Exercise system
            var result = fixture.Get((object p) => sut.EditProductCommand.CanExecute(p));
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void EditProductCommandCanNotExecuteWhenNoProductIsSelected(MainWindowViewModel sut, object p)
        {
            // Fixture setup
            // Exercise system
            var result = sut.EditProductCommand.CanExecute(p);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void ExecuteEditProductCommandWillShowCorrectDialog()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();
            var product = fixture.Build<ProductViewModel>()
                .With(x => x.IsSelected, true)
                .CreateAnonymous();
            var expectedEditor = product.Edit().AsSource().OfLikeness<ProductEditorViewModel>()
                .With(d => d.Title).EqualsWhen((s, d) => "Edit Product" == d.Title);

            var childWindowMock = fixture.CreateAnonymous<Mock<IWindow>>();
            var windowStub = fixture.Freeze<Mock<IWindow>>();
            windowStub.Setup(w => w.CreateChild(expectedEditor))
                .Returns(childWindowMock.Object);

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            sut.Products.Add(product);
            // Exercise system
            fixture.Do((object p) => sut.EditProductCommand.Execute(p));
            // Verify outcome
            childWindowMock.Verify(w => w.ShowDialog());
            // Teardown
        }

        [Fact]
        public void ExecuteEditProductCommandWillNotUpdateOnAgentWhenReturnValueIsNull()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();

            fixture.Freeze<Mock<IWindow>>().Setup(w => w.CreateChild(It.IsAny<object>())).Returns(() =>
                {
                    var childStub = fixture.CreateAnonymous<Mock<IWindow>>();
                    childStub.Setup(cw => cw.ShowDialog()).Returns((bool?)null);
                    return childStub.Object;
                });

            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            sut.Products.Add(fixture.Build<ProductViewModel>()
                .With(x => x.IsSelected, true)
                .CreateAnonymous());
            // Exercise system
            fixture.Do((object p) => sut.EditProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.InsertProduct(It.IsAny<ProductEditorViewModel>()), Times.Never());
            // Teardown
        }

        [Fact]
        public void ExecuteEditProductCommandWillNotUpdateOnAgentWhenReturnValueIsFalse()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();

            fixture.Freeze<Mock<IWindow>>().Setup(w => w.CreateChild(It.IsAny<object>())).Returns(() =>
                {
                    var childStub = fixture.CreateAnonymous<Mock<IWindow>>();
                    childStub.Setup(cw => cw.ShowDialog()).Returns(false);
                    return childStub.Object;
                });

            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            sut.Products.Add(fixture.Build<ProductViewModel>()
                .With(x => x.IsSelected, true)
                .CreateAnonymous());
            // Exercise system
            fixture.Do((object p) => sut.EditProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.InsertProduct(It.IsAny<ProductEditorViewModel>()), Times.Never());
            // Teardown
        }

        [Fact]
        public void ExecuteEditProductCommandWillUpdateOnAgentWhenReturnValueIsTrue()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();
            var product = fixture.Build<ProductViewModel>()
                .With(x => x.IsSelected, true)
                .CreateAnonymous();
            var editor = product.Edit().AsSource().OfLikeness<ProductEditorViewModel>()
                .With(d => d.Title).EqualsWhen((s, d) => "Edit Product" == d.Title)
                .Without(d => d.Error)
                .Without(d => d.IsValid)
                .Without(d => d.Title);

            fixture.Freeze<Mock<IWindow>>()
                .Setup(w => w.CreateChild(It.IsAny<object>()))
                .Callback((object vm) =>
                    {
                        var pVM = (ProductEditorViewModel)vm;
                        pVM.Currency = editor.Value.Currency;
                        pVM.Name = editor.Value.Name;
                        pVM.Price = editor.Value.Price;
                    })
                .Returns(() =>
                    {
                        var childStub = fixture.CreateAnonymous<Mock<IWindow>>();
                        childStub.Setup(cw => cw.ShowDialog()).Returns(true);
                        return childStub.Object;
                    });

            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            sut.Products.Add(product);
            // Exercise system
            fixture.Do((object p) => sut.EditProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.UpdateProduct(It.Is<ProductEditorViewModel>(pvm => editor.Equals(pvm))));
            // Teardown
        }

        [Fact]
        public void ExecuteEditProductCommandWillReloadProducts()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();

            fixture.Freeze<Mock<IWindow>>().Setup(w => w.CreateChild(It.IsAny<object>())).Returns(() =>
                {
                    var childStub = fixture.CreateAnonymous<Mock<IWindow>>();
                    childStub.Setup(cw => cw.ShowDialog()).Returns(true);
                    return childStub.Object;
                });
            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            sut.Products.Add(fixture.Build<ProductViewModel>()
                .With(x => x.IsSelected, true)
                .CreateAnonymous());
            // Exercise system
            fixture.Do((object p) => sut.EditProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.SelectAllProducts());
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DeleteProductCommandIsInstance(MainWindowViewModel sut)
        {
            // Fixture setup
            // Exercise system
            ICommand result = sut.DeleteProductCommand;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void DeleteProductCommandCanExecuteWhenProductIsSelected()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();
            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            sut.Products.Add(fixture.Build<ProductViewModel>()
                .With(x => x.IsSelected, true)
                .CreateAnonymous());
            // Exercise system
            var result = fixture.Get((object p) => sut.DeleteProductCommand.CanExecute(p));
            // Verify outcome
            Assert.True(result, "DeleteProductCommand");
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DeleteProductCommandCanNotExecuteWhenNoProductIsSelected(MainWindowViewModel sut, object p)
        {
            // Fixture setup
            // Exercise system
            var result = sut.DeleteProductCommand.CanExecute(p);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void ExecuteDeleteProductCommandWillDeleteFromAgent()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();
            var product = fixture.Build<ProductViewModel>()
                .With(x => x.IsSelected, true)
                .CreateAnonymous();

            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            sut.Products.Add(product);
            // Exercise system
            fixture.Do((object p) => sut.DeleteProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.DeleteProduct(product.Id));
            // Teardown
        }

        [Fact]
        public void ExecuteDeleteProductCommandWillReloadProducts()
        {
            // Fixture setup
            var fixture = new AutoMoqFixture();
            var product = fixture.Build<ProductViewModel>()
                .With(x => x.IsSelected, true)
                .CreateAnonymous();

            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();

            var sut = fixture.CreateAnonymous<MainWindowViewModel>();
            sut.Products.Add(product);
            // Exercise system
            fixture.Do((object p) => sut.DeleteProductCommand.Execute(p));
            // Verify outcome
            agentMock.Verify(a => a.SelectAllProducts());
            // Teardown
        }
    }
}
