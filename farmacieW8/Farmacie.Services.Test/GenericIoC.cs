using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Farmacie.Models;
using Farmacie.Services;
using Farmacie.Services.Impl;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Farmacie.Test
{
    public static class GenericIoC
    {
        public static IContainer BaseContainer { get; private set; }

        public static Action<ContainerBuilder> InjectDependency { get; set; }

        public static void Build()
        {
            if (BaseContainer == null)
            {
                var builder = new ContainerBuilder();

                //builder.Register(a => new Frame()).SingleInstance();
                //builder.RegisterType<FarmacieService>().As<IFarmacieService>();
                //builder.RegisterType<RegioniService>().As<IRegioniService>();
                InjectDependency(builder);

                BaseContainer = builder.Build();
            }
        }

        public static TService Resolve<TService>()
        {
            return BaseContainer.Resolve<TService>();
        }
    }
}
