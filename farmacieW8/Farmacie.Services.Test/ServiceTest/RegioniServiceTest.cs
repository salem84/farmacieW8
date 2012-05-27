using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Farmacie.Services.Impl;
using Farmacie.Services;
using Farmacie.Models;
using Autofac;

namespace Farmacie.Test.ServiceTest
{
    [TestClass]
    public class RegioniServiceTest
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
                    builder.RegisterType<RegioniService>().As<IRegioniService>();
                });

            GenericIoC.Build();
        }

        [TestMethod]
        public void LeggiRegioni()
        {
            var service = GenericIoC.Resolve<IRegioniService>();
            var result = service.GetRegioni();

            int count = result.Count();
            Assert.IsTrue(count == 21);
        }
    }
}