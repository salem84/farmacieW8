using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Farmacie.Models;
using Farmacie.Services;
using Farmacie.Services.Impl;
using Farmacie.App.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Farmacie.App
{
    public static class IoC
    {
        public static IContainer BaseContainer { get; private set; }

        public static void Build()
        {
            if (BaseContainer == null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<RegioniService>().As<IRegioniService>();

                //builder.Register(a => new Frame()).SingleInstance();

                var rootFrame = new Frame();
                builder.Register(a => new NavigationService(rootFrame)).SingleInstance();
                builder.Register(a => new MainFunctions()).SingleInstance();

                BaseContainer = builder.Build();


                var funzioniService = new MainFunctions();

                rootFrame.Navigate(typeof(ItemsPage), funzioniService.GetAllFunctions());



                Window.Current.Content = rootFrame;
                Window.Current.Activate();
            }
        }

        public static TService Resolve<TService>()
        {
            return BaseContainer.Resolve<TService>();
        }
    }
}
