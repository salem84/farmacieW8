using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Farmacie.Models;
using Farmacie.Services.Impl;
using Farmacie.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Farmacie.Test.ServiceTest
{
    [TestClass]
    public class CacheTest
    {
        Cache<Farmacia> cache;
        int callService;
        
        RegioniFake regioniFake;
        
        Expression<Func<Farmacia, bool>> queryLazio, queryPiemonte;

        #region INIT

        [TestInitialize]
        public void Init()
        {
            InitCache();
            regioniFake = new RegioniFake();
            InitQuery();
        }

        public void InitCache()
        {
            cache = new Cache<Farmacia>();
            callService = 0;

            //Func<Func<Farmacia, bool>, IEnumerable<Farmacia>> funzione = ServiziFake.RichiamaServizio;
            
            cache.GetValues = (Func<Farmacia, bool> query) =>
            {
                callService++;
                return ServiziOgdiFake.RichiamaServizio(query);
            };
        }

        

        public void InitQuery()
        {
            queryLazio = f => f.Regione == regioniFake.regLazio;
            queryPiemonte = f => f.Regione == regioniFake.regPiemonte;
        }

        #endregion


        [TestMethod]
        public void RestituzioneElementiLazio()
        {
            var r = cache.Where(f => f.Regione == regioniFake.regLazio);

            Assert.IsNotNull(r);

            Assert.IsTrue(r.Count() == Costanti.NUM_FARMACIE_LAZIO);
        }

        [TestMethod]
        public void RestituzioneElementiPiemonte()
        {
            var r = cache.Where(f => f.Regione == regioniFake.regPiemonte);

            Assert.IsNotNull(r);

            Assert.IsTrue(r.Count() == Costanti.NUM_FARMACIE_PIEMONTE);
        }

        #region VERIFICA CACHE

        [TestMethod]
        public void VerificaRecuperoInCache_CacheVuota()
        {
            // Cache vuota => chiamo 1 volta i servizi
            var r = cache.Where(queryLazio);
            Assert.IsTrue(callService == 1);
        }

        [TestMethod]
        public void VerificaRecuperoInCache_StessaRegioneStessaLambda()
        {
            VerificaRecuperoInCache_CacheVuota();

            // Rifaccio la query con lo stessa regione e stesse lambda expression 
            // => riutilizzo la cache e non chiamo i servizi
            var r2 = cache.Where(queryLazio);
            Assert.IsTrue(callService == 1);
        }

        [TestMethod]
        public void VerificaRecuperoInCache_StessaRegioneLambdaConPuntamentiDifferenti()
        {
            VerificaRecuperoInCache_CacheVuota();

            // Rifaccio la query con lo stessa regione e lambda expression uguali ma con reference diverse
            // => riutilizzo la cache e non chiamo i servizi
            cache.Where(f => f.Regione == regioniFake.regLazio);
            Assert.IsTrue(callService == 1);
        }

        [TestMethod]
        public void VerificaRecuperoInCache_RegioniDiverse()
        {
            VerificaRecuperoInCache_CacheVuota();

            // Rifaccio la query con lo stessa regione e lambda expression uguali ma con reference diverse
            // => riutilizzo la cache e non chiamo i servizi
            cache.Where(f => f.Regione == regioniFake.regPiemonte);
            Assert.IsTrue(callService == 2);
        }

        #endregion

        [TestMethod]
        public void VerificaRecuperInCache_Costanti()
        {
            cache.Where(f => f.partitaiva == "100");
            
            cache.Where(f => f.partitaiva == "100");
            Assert.IsTrue(callService == 1);

            cache.Where(f => f.partitaiva == "200");
            Assert.IsTrue(callService == 2);
        }

        [TestMethod]
        public void VerificaRecuperInCache_CostantiCombinate()
        {
            cache.Where(f => f.partitaiva == "100" && f.partitaiva == "200");

            cache.Where(f => (f.partitaiva == "100" && f.partitaiva == "200"));
            Assert.IsTrue(callService == 1);
        }



        [TestMethod]
        public void VerificaRecuperInCache_IstanzeOggettiDiversiMaCongruenti()
        {
            string p1 = "100";
            string p2 = "100";
            string p3 = "200";

            cache.Where(f => f.partitaiva == p1);

            cache.Where(f => f.partitaiva == p2);
            Assert.IsTrue(callService == 1);

            cache.Where(f => f.partitaiva == p1);
            Assert.IsTrue(callService == 1);

            cache.Where(f => f.partitaiva == p3);
            Assert.IsTrue(callService == 2);
        }
    }
}
