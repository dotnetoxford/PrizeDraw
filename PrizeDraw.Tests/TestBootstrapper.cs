using Autofac;
using PrizeDraw.Helpers;
using PrizeDraw.TileRepositories;
using PrizeDraw.TileRepositories.Impl;

namespace PrizeDraw.Tests
{
    public static class TestBootstrapper
    {
        public static IContainer Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TileRepositoryFactory>().As<ITileRepositoryFactory>().SingleInstance();
            builder.RegisterType<MeetupComHelper>().As<IMeetupComHelper>();

            return builder.Build();
        }
    }
}