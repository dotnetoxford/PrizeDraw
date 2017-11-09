using Autofac;
using PrizeDraw.Helpers;

namespace PrizeDraw.Tests
{
    public static class TestBootstrapper
    {
        public static IContainer Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TileProviderFactory>().As<ITileProviderFactory>().SingleInstance();
            builder.RegisterType<MeetupComHelper>().As<IMeetupComHelper>();

            return builder.Build();
        }
    }
}