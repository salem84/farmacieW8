using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Farmacie.Models;
using Farmacie.Test.Mock;

namespace Farmacie.Test.ModelTest
{
    [TestClass]
    public class RegioneModelTest
    {
        RegioniFake R;

        [TestInitialize]
        public void InitRegioni()
        {
            R = new RegioniFake();
        }


        [TestMethod]
        public void ConfrontaConEquals()
        {
            bool result;

            // confronta con se stesso
            result = R.regLazio.Equals(R.regLazio);

            Assert.IsTrue(result);

            // confronta con uno uguale
            result = R.regLazio.Equals(R.regLazio2);

            Assert.IsTrue(result);

            // confronta con uno diverso
            result = R.regLazio.Equals(R.regPiemonte);

            Assert.IsFalse(result);
        }


        [TestMethod]
        public void ConfrontaConEqualityComparer()
        {
            bool result;
            
            // confronta con se stesso
            result = EqualityComparer<Regione>.Default.Equals(R.regLazio, R.regLazio);
            Assert.IsTrue(result);

            // confronta con uno uguale
            result = EqualityComparer<Regione>.Default.Equals(R.regLazio, R.regLazio2);
            Assert.IsTrue(result);

            // confronta con uno diverso
            result = EqualityComparer<Regione>.Default.Equals(R.regLazio, R.regPiemonte);
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void ConfrontaConOperatore()
        {
            bool result;

            // confronta con se stesso
            result = R.regLazio == R.regLazio;
            Assert.IsTrue(result);

            // confronta con uno uguale
            result = R.regLazio == R.regLazio2;
            Assert.IsTrue(result);

            // confronta con uno diverso
            result = R.regLazio == R.regPiemonte;
            Assert.IsFalse(result);

            result = R.regLazio != R.regPiemonte;
            Assert.IsTrue(result);

        }
    }
}