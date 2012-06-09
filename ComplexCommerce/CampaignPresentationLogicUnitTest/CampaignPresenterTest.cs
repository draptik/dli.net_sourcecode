using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;
using Moq;
using Xunit.Extensions;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Commerce.CampaignPresentation.UnitTest
{
    public class CampaignPresenterTest
    {
        [Fact]
        public void SelectAllWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var items = fixture.CreateMany<CampaignItem>().ToList();
            var expectedPresenters = fixture.CreateMany<CampaignItemPresenter>().ToList();

            fixture.Freeze<Mock<CampaignRepository>>().Setup(r => r.SelectAll()).Returns(items);
            fixture.Freeze<Mock<IPresentationMapper>>().Setup(m => m.Map(items)).Returns(expectedPresenters);

            var sut = fixture.CreateAnonymous<CampaignPresenter>();
            // Exercise system
            IEnumerable<CampaignItemPresenter> result = sut.SelectAll();
            // Verify outcome
            Assert.Equal(expectedPresenters, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void UpdateNullPresenterWillThrow(CampaignPresenter sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Update(null));
            // Teardown
        }

        [Fact]
        public void UpdateWillUpdateRepository()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var presenter = fixture.CreateAnonymous<CampaignItemPresenter>();
            var item = fixture.CreateAnonymous<CampaignItem>();

            fixture.Freeze<Mock<IPresentationMapper>>().Setup(m => m.Map(presenter)).Returns(item);
            var repositoryMock = fixture.Freeze<Mock<CampaignRepository>>();

            var sut = fixture.CreateAnonymous<CampaignPresenter>();
            // Exercise system
            sut.Update(presenter);
            // Verify outcome
            repositoryMock.Verify(r => r.Update(item));
            // Teardown
        }
    }
}
