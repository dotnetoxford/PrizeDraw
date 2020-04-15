using Autofac;
using GalaSoft.MvvmLight.Views;
using PrizeDraw.Helpers;
using PrizeDraw.TIleProviders;
using PrizeDraw.TIleProviders.Impl;
using PrizeDraw.ViewModels;

namespace PrizeDraw
{
    public static class Bootstrapper
    {
        public static IContainer Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>();
            builder.RegisterType<MainWindowViewModel>();

            builder.RegisterType<MeetupComSyncDialog>().As<IMeetupComSyncDialog>();
            builder.RegisterType<MeetupDotComSync>();
            builder.RegisterType<MeetupDotComSyncViewModel>();
            builder.RegisterType<RequestEventIdDialogViewModel>();

            builder.RegisterType<TileProviderFactory>().As<ITileProviderFactory>().SingleInstance();
            builder.RegisterType<WavSoundEffects>().As<ISoundEffects>().SingleInstance();
            builder.RegisterType<MeetupComEventValidator>().As<IEventValidator>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<MeetupComHelper>().As<IMeetupComHelper>();
            builder.RegisterType<MeetupComEventValidator>().As<IEventValidator>();

            return builder.Build();
        }
    }
}