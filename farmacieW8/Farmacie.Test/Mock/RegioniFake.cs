using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmacie.Models;

namespace Farmacie.Test.Mock
{
    public class RegioniFake
    {
        public Regione regLazio, regLazio2, regPiemonte;

        public RegioniFake()
        {
            regLazio = new Regione("120", "Lazio");
            regLazio2 = new Regione("120", "Lazio");
            regPiemonte = new Regione("10", "Piemonte");
        }
    }
}
