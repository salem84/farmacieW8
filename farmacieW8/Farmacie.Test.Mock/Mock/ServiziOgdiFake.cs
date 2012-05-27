using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmacie.Models;

namespace Farmacie.Test.Mock
{
    public class ServiziOgdiFake
    {
        public static IEnumerable<Farmacia> RichiamaServizio(Func<Farmacia, bool> query)
        {
            List<Farmacia> lista = new List<Farmacia>();
            for (int i = 0; i < Costanti.NUM_FARMACIE_LAZIO; i++)
            {
                lista.Add(new Farmacia()
                {
                    Regione = new Regione("120", "Lazio"),
                    partitaiva = (100+i).ToString()
                });
            }

            for (int i = 0; i < Costanti.NUM_FARMACIE_PIEMONTE; i++)
            {
                lista.Add(new Farmacia()
                {
                    Regione = new Regione("10", "Piemonte"),
                    partitaiva = (500 + i).ToString()
                });
            }

            return lista;
        }
    }
}
