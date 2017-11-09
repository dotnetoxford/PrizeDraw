using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using PrizeDraw.Helpers;

namespace PrizeDraw.Tests.Integration.Helpers
{
    [TestFixture, Category("Integration")]
    public class AttendeeMeetupComTileProviderTests
    {
        [Test]
        public async Task GivenAMeetupEventId_WithRSVPs_ThenResultsShouldHaveValues()
        {
            var container = TestBootstrapper.Init();

            var factory = container.Resolve<ITileProviderFactory>();


            var sut = factory.CreateMeetupComTileProvider(242414971);
            var tiles = await sut.GetTilesAsync();

            Assert.That(tiles.Count, Is.GreaterThan(0));
        }
    }
}
