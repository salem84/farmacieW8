using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Farmacie.Models;
using Farmacie.Services.Impl;
using Farmacie.Services;

namespace Farmacie.Test.ServiceTest
{
    [TestClass]
    public class FarmacieServiceTest
    {
        [TestInitialize]
        public void Init()
        {
            InitIoC();
        }

        private void InitIoC()
        {
            GenericIoC.InjectDependency = (builder =>
            {
                builder.Register(a => new Cache<Farmacia>()).SingleInstance();
                builder.RegisterType<FarmacieService>().As<IFarmacieService>();
                builder.RegisterType<RegioniService>().As<IRegioniService>();
            });
            GenericIoC.Build();
        }

        [TestMethod]
        public void LeggiFarmacie()
        {
            var regione1 = GenericIoC.Resolve<IRegioniService>().GetRegioni().First();
            var regione2 = GenericIoC.Resolve<IRegioniService>().GetRegioni().Last();
            var fService = GenericIoC.Resolve<IFarmacieService>();

            fService.GetFarmaciaByRegione(regione1);


            var fService2 = GenericIoC.Resolve<IFarmacieService>();
            fService2.GetFarmaciaByRegione(regione2);
        }
    }
}