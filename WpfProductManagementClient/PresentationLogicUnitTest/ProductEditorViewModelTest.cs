using System;
using System.ComponentModel;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf.UnitTest
{
    public class ProductEditorViewModelTest
    {
        [Fact]
        public void IdIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedId = fixture.Freeze<int>();
            var sut = fixture.Build<ProductEditorViewModel>().OmitAutoProperties().CreateAnonymous();
            // Exercise system
            var result = sut.Id;
            // Verify outcome
            Assert.Equal(expectedId, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void NameIsProperWritableProperty(string expectedName, ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system
            sut.Name = expectedName;
            string result = sut.Name;
            // Verify outcome
            Assert.Equal(expectedName, result);
            // Teardown
        }

        [Fact]
        public void DefaultNameIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<ProductEditorViewModel>().OmitAutoProperties().CreateAnonymous();
            // Exercise system
            var result = sut.Name;
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void SettingNullNameWillThrow(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Name = null);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void CurrencyIsProperWritableProperty(string expectedCurrency, ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system
            sut.Currency = expectedCurrency;
            string result = sut.Currency;
            // Verify outcome
            Assert.Equal(expectedCurrency, result);
            // Teardown
        }

        [Fact]
        public void DefaultCurrencyIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<ProductEditorViewModel>().OmitAutoProperties().CreateAnonymous();
            // Exercise system
            var result = sut.Currency;
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void SettingNullCurrencyWillThrow(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Currency = null);
            // Teardown
        }

        [Fact]
        public void PriceIsProperWritableProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedPrice = fixture.CreateAnonymous<decimal>().ToString();
            var sut = fixture.CreateAnonymous<ProductEditorViewModel>();
            // Exercise system
            sut.Price = expectedPrice;
            string result = sut.Price;
            // Verify outcome
            Assert.Equal(expectedPrice, result);
            // Teardown
        }

        [Fact]
        public void DefaultPriceIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<ProductEditorViewModel>().OmitAutoProperties().CreateAnonymous();
            // Exercise system
            var result = sut.Price;
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void SettingNullPriceWillThrow(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Price = null);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void TitleIsProperWritableProperty(string expectedTitle, ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system
            sut.Title = expectedTitle;
            string result = sut.Title;
            // Verify outcome
            Assert.Equal(expectedTitle, result);
            // Teardown
        }

        [Fact]
        public void DefaultTitleIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<ProductEditorViewModel>().OmitAutoProperties().CreateAnonymous();
            // Exercise system
            var result = sut.Title;
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void SettingNullTitleWillThrow(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Title = null);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void SutIsDataErrorInfo(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IDataErrorInfo>(sut);
            // Teardown
        }

        [Fact]
        public void ErrorForCurrencyIsCorrectWhenCurrencyIsDkk()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.GetValidationResult<ProductEditorViewModel, string>(sut => sut.Currency, "DKK");
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Fact]
        public void ErrorForCurrencyIsCorrectWhenCurrencyIsUsd()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.GetValidationResult<ProductEditorViewModel, string>(sut => sut.Currency, "USD");
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Fact]
        public void ErrorForCurrencyIsCorrectWhenCurrencyIsEur()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.GetValidationResult<ProductEditorViewModel, string>(sut => sut.Currency, "EUR");
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Fact]
        public void ErrorForCurrencyIsCorrectWhenCurrencyIsInvalid()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.GetValidationResult<ProductEditorViewModel, string>(sut => sut.Currency, fixture.CreateAnonymous("Currency"));
            // Verify outcome
            Assert.Equal("Currency must be either DKK, USD or EUR", result);
            // Teardown
        }

        [Fact]
        public void ErrorForNameIsCorrectWhenNameIsValid()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.GetValidationResult<ProductEditorViewModel, string>(sut => sut.Name, fixture.CreateAnonymous("Name").First().ToString());
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Fact]
        public void ErrorForNameIsCorrectWhenNameIsInvalid()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.GetValidationResult<ProductEditorViewModel, string>(sut => sut.Name, string.Empty);
            // Verify outcome
            Assert.Equal("Name must be at least one character long.", result);
            // Teardown
        }

        [Fact]
        public void ErrorForPriceIsCorrectWhenPriceIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var correctPrice = fixture.CreateAnonymous<decimal>().ToString();
            // Exercise system
            var result = fixture.GetValidationResult<ProductEditorViewModel, string>(sut => sut.Price, correctPrice);
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Fact]
        public void ErrorForPriceIsCorrectWhenPriceIsIncorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var incorrectPrice = fixture.CreateAnonymous("Price");
            // Exercise system
            var result = fixture.GetValidationResult<ProductEditorViewModel, string>(sut => sut.Price, incorrectPrice);
            // Verify outcome
            Assert.Equal("Price must be a number.", result);
            // Teardown
        }

        [Fact]
        public void ErrorIsCorrectWhenSutIsValid()
        {
            // Fixture setup
            var fixture = new Fixture();
            var correctCurrency = "DKK";
            var correctPrice = fixture.CreateAnonymous<decimal>().ToString();

            var sut = fixture.Build<ProductEditorViewModel>()
                .With(x => x.Currency, correctCurrency)
                .With(x => x.Price, correctPrice)
                .CreateAnonymous();
            // Exercise system
            var result = sut.Error;
            // Verify outcome
            Assert.Equal(string.Empty, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ErrorIsCorrectWhenSutIsInvalid(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.Error;
            // Verify outcome
            Assert.Equal("One or more values are invalid.", result);
            // Teardown
        }

        [Fact]
        public void IsValidIsTrueWhenSutIsValid()
        {
            // Fixture setup
            var fixture = new Fixture();
            var correctCurrency = "DKK";
            var correctPrice = fixture.CreateAnonymous<decimal>().ToString();

            var sut = fixture.Build<ProductEditorViewModel>()
                .With(x => x.Currency, correctCurrency)
                .With(x => x.Price, correctPrice)
                .CreateAnonymous();
            // Exercise system
            var result = sut.IsValid;
            // Verify outcome
            Assert.True(result, "IsValid");
            // Teardown
        }

        [Theory, AutoMoqData]
        public void IsValidIsFalseWhenSutIsInvalid(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.IsValid;
            // Verify outcome
            Assert.False(result, "IsValid");
            // Teardown
        }

        [Theory, AutoMoqData]
        public void SutIsNotifyPropertyChanged(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<INotifyPropertyChanged>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ChangingCurrencyWillRaiseNotifyEvent(ProductEditorViewModel sut, string currency)
        {
            // Fixture setup
            // Exercise system and verify outcome
            sut.ShouldNotifyOn(s => s.Currency).When(s => s.Currency =  currency);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void NotChangingCurrencyWillNotRaiseNotifyEvent(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            sut.ShouldNotNotifyOn(s => s.Currency).When(s => s.Currency = s.Currency);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ChangingNameWillRaiseNotifyEvent(ProductEditorViewModel sut, string name)
        {
            // Fixture setup
            // Exercise system and verify outcome
            sut.ShouldNotifyOn(s => s.Name).When(s => s.Name = name);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void NotChangingNameWillNotRaiseNotifyEvent(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            sut.ShouldNotNotifyOn(s => s.Name).When(s => s.Name = s.Name);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ChangingPriceWillRaiseNotifyEvent(ProductEditorViewModel sut, string price)
        {
            // Fixture setup
            // Exercise system and verify outcome
            sut.ShouldNotifyOn(s => s.Price).When(s => s.Price = price);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void NotChangingPriceWillNotRaiseNotifyEvent(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            sut.ShouldNotNotifyOn(s => s.Price).When(s => s.Price = s.Price);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ChangingTitleWillRaiseNotifyEvent(ProductEditorViewModel sut, string title)
        {
            // Fixture setup
            // Exercise system and verify outcome
            sut.ShouldNotifyOn(s => s.Title).When(s => s.Title = title);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void NotChangingTitleWillNotRaiseNotifyEvent(ProductEditorViewModel sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            sut.ShouldNotNotifyOn(s => s.Title).When(s => s.Title = s.Title);
            // Teardown
        }

        [Fact]
        public void ChangingFromInvalidToValidStateWillRaiseNotifyEvent()
        {
            // Fixture setup
            var fixture = new Fixture();
            var correctCurrency = "DKK";
            var correctPrice = fixture.CreateAnonymous<decimal>().ToString();

            var sut = fixture.Build<ProductEditorViewModel>()
                .With(s => s.Price, correctPrice)
                .CreateAnonymous();
            // Exercise system and verify outcome
            sut.ShouldNotifyOn(s => s.IsValid).When(s => s.Currency = correctCurrency);
            // Teardown
        }

        [Fact]
        public void ChangingFromValidToInvalidStateWillRaiseNotifyEvent()
        {
            // Fixture setup
            var fixture = new Fixture();
            var correctCurrency = "DKK";
            var correctPrice = fixture.CreateAnonymous<decimal>().ToString();

            var sut = fixture.Build<ProductEditorViewModel>()
                .With(s => s.Currency, correctCurrency)
                .With(s => s.Price, correctPrice)
                .CreateAnonymous();
            // Exercise system and verify outcome
            sut.ShouldNotifyOn(s => s.IsValid).When(s => s.Price = fixture.CreateAnonymous("Price"));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ModificationThatDoesNotChangeValidStateWillNotRaiseNotifyEvent(ProductEditorViewModel sut, string price)
        {
            // Fixture setup
            // Exercise system and verify outcome
            sut.ShouldNotNotifyOn(s => s.IsValid).When(s => s.Price = price);
            // Teardown
        }
    }
}
