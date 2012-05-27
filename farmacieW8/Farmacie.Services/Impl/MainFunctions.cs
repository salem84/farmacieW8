using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmacie.Models;

namespace Farmacie.Services.Impl
{
    public class MainFunctions
    {
        private List<Farmacia> _farmacie;

        public ICollection<Funzione> GetAllFunctions()
        {
            var mg = new List<Funzione>();
            mg.Add(new Funzione
            {
                Title = "Farmacie",
                Subtitle = "Elenco delle farmacie"
            });

            return mg;
        }
    }
}
