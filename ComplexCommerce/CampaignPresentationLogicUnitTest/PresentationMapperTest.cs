using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison.Fluent;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Commerce.CampaignPresentation.UnitTest
{
    public class PresentationMapperTest
    {
        [Theory, AutoMoqData]
        public void SutIsPresentationMapper(PresentationMapper sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IPresentationMapper>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapNullItemWillThrow(PresentationMapper sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((CampaignItem)null));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapCampaignItemWithDiscountWillReturnCorrectResult(CampaignItem item, PresentationMapper sut)
        {
            // Fixture setup
            var expectedPresenter = item.AsSource().OfLikeness<CampaignItemPresenter>()
                .With(d => d.Id).EqualsWhen((s, d) => s.Product.Id == d.Id)
                .With(d => d.ProductName).EqualsWhen((s, d) => s.Product.Name == d.ProductName)
                .With(d => d.UnitPrice).EqualsWhen((s, d) => s.Product.UnitPrice.Amount == d.UnitPrice)
                .With(d => d.DiscountPrice).EqualsWhen((s, d) => s.DiscountPrice.Amount == d.DiscountPrice);
            // Exercise system
            var result = sut.Map(item);
            // Verify outcome
            Assert.True(expectedPresenter.Equals(result));
            // Teardown
        }

        [Fact]
        public void MapCampaignItemWithoutDiscountWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Freeze<Product>();
            fixture.Inject((Money)null);

            var item = fixture.CreateAnonymous<CampaignItem>();

            var expectedPresenter = item.AsSource().OfLikeness<CampaignItemPresenter>()
                .With(d => d.Id).EqualsWhen((s, d) => s.Product.Id == d.Id)
                .With(d => d.ProductName).EqualsWhen((s, d) => s.Product.Name == d.ProductName)
                .With(d => d.UnitPrice).EqualsWhen((s, d) => s.Product.UnitPrice.Amount == d.UnitPrice)
                .With(d => d.DiscountPrice).EqualsWhen((s, d) => object.Equals(s.DiscountPrice, d.DiscountPrice));

            var sut = fixture.CreateAnonymous<PresentationMapper>();
            // Exercise system
            var result = sut.Map(item);
            // Verify outcome
            Assert.True(expectedPresenter.Equals(result));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapNullCampaignItemsWillThrow(PresentationMapper sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((IEnumerable<CampaignItem>)null));
            // Teardown
        }

        [Fact]
        public void MapCampaignItemsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var items = fixture.CreateMany<CampaignItem>().ToList();
            var expectedPresenters = fixture.CreateMany<CampaignItemPresenter>().ToList();
            var queue = new Queue<CampaignItemPresenter>(expectedPresenters);

            var sutStub = fixture.CreateAnonymous<Mock<PresentationMapper>>();
            sutStub.Setup(s => s.Map(It.Is<CampaignItem>(ci => items.Contains(ci)))).Returns(queue.Dequeue);
            // Exercise system
            var result = sutStub.Object.Map(items);
            // Verify outcome
            Assert.True(expectedPresenters.SequenceEqual(result));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapNullCampaignItemPresenterWillThrow(PresentationMapper sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((CampaignItemPresenter)null));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapCampaignItemPresenterWithDiscountWillReturnCorrectResult(CampaignItemPresenter presenter, PresentationMapper sut)
        {
            // Fixture setup
            var expectedItem = presenter.AsSource().OfLikeness<CampaignItem>()
                .With(d => d.DiscountPrice).EqualsWhen((s, d) => s.DiscountPrice.Value == d.DiscountPrice.Amount && d.DiscountPrice.CurrencyCode == "DKK")
                .With(d => d.Product).EqualsWhen((s, d) => s.AsSource().OfLikeness<Product>()
                    .With(d1 => d1.UnitPrice).EqualsWhen((s1, d1) => s1.UnitPrice == d1.UnitPrice.Amount && d1.UnitPrice.CurrencyCode == "DKK")
                    .With(d1 => d1.Name).EqualsWhen((s1, d1) => s1.ProductName == d1.Name)
                .Equals(d.Product));
            // Exercise system
            var result = sut.Map(presenter);
            // Verify outcome
            Assert.True(expectedItem.Equals(result));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapCampaignItemPresenterWithoutDiscountWillReturnCorrectResult(CampaignItemPresenter presenter, PresentationMapper sut)
        {
            // Fixture setup
            presenter.DiscountPrice = null;

            var expectedItem = presenter.AsSource().OfLikeness<CampaignItem>()
                .With(d => d.DiscountPrice).EqualsWhen((s, d) => null == d.DiscountPrice)
                .With(d => d.Product).EqualsWhen((s, d) => s.AsSource().OfLikeness<Product>()
                    .With(d1 => d1.UnitPrice).EqualsWhen((s1, d1) => s1.UnitPrice == d1.UnitPrice.Amount && d1.UnitPrice.CurrencyCode == "DKK")
                    .With(d1 => d1.Name).EqualsWhen((s1, d1) => s1.ProductName == d1.Name)
                .Equals(d.Product));
            // Exercise system
            var result = sut.Map(presenter);
            // Verify outcome
            Assert.True(expectedItem.Equals(result));
            // Teardown
        }
    }
}
