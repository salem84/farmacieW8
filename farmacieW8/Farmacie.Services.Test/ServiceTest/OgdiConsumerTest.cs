using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Farmacie.Services.Impl;
using Farmacie.Test.Mock;

namespace Farmacie.Test.ServiceTest
{
    [TestClass]
    public class OgdiConsumerTest
    {
        private OgdiConsumer ogdiConsumer;

        [TestInitialize]
        public void Init()
        {
            InitService();
        }

        private void InitService()
        {
            ogdiConsumer = new OgdiConsumer()
            {
                DatasetUrl = "http://opendatasalutedata.cloudapp.net/v1/datacatalog/Farmacie/"
            };
        }

        [TestMethod]
        public void InterrogaServizio()
        {
            int chiamate = 0;
            List<FarmaciaFake> farmacie = new List<FarmaciaFake>();

            Task t = new Task(new Action(() =>
            {    
                bool loadMore = true;

                while (ogdiConsumer.LoadNextDataChunkAsync(5, farmacie).Result == true && loadMore == true)
                {
                    chiamate++;

                    if (chiamate > 5)
                        break;
                }               
            }));

            t.Start();
            t.Wait();

            Assert.IsTrue(chiamate > 0);

            int count = farmacie.Count();
            Assert.IsTrue(count > 0);

            int randomIndex = (new Random()).Next(count-1);
            var farmacia = farmacie[randomIndex];

            Assert.IsFalse(string.IsNullOrEmpty(farmacia.partitaiva));
        }


        [TestMethod]
        public void InterrogaServizioConQuery()
        {
            ogdiConsumer.QueryFilter = "descrizioneregione eq 'PIEMONTE'";

            int chiamate = 0;
            List<FarmaciaFake> farmacie = new List<FarmaciaFake>();

            Task t = new Task(new Action(() =>
            {
                bool loadMore = true;

                while (ogdiConsumer.LoadNextDataChunkAsync(5, farmacie).Result == true && loadMore == true)
                {
                    chiamate++;

                    if (chiamate > 5)
                        break;
                }
            }));

            t.Start();
            t.Wait();

            Assert.IsTrue(chiamate > 0);

            int count = farmacie.Count();
            Assert.IsTrue(count > 0);

            int randomIndex = (new Random()).Next(count - 1);
            var farmacia = farmacie[randomIndex];

            Assert.IsFalse(string.IsNullOrEmpty(farmacia.partitaiva));
        }
    }
}