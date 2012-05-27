using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmacie.Models;

namespace Farmacie.Services
{
    public interface IFarmacieService
    {
        IEnumerable<Farmacia> GetAllFarmacie();
        IEnumerable<Farmacia> GetFarmaciaByRegione(Regione regione);
    }
}
