using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using PrizeDraw.TileRepositories;
using Xunit;

namespace PrizeDraw.Tests.Integration.Helpers
{
    public class AttendeeMeetupComTileRepositoryTests
    {
        [Fact]
        public async Task GivenAMeetupEventId_WithRSVPs_ThenResultsShouldHaveValues()
        {
            var container = TestBootstrapper.Init();

            var factory = container.Resolve<ITileRepositoryFactory>();

            var sut = factory.CreateMeetupComTileRepository("242414971");
            var tiles = await sut.GetTilesAsync();

            tiles.Count.Should().BeGreaterThan(0);
        }
    }
}
