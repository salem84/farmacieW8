using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Farmacie.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Farmacie.Services.Impl
{
    public class RegioniService : IRegioniService
    {
        private const string RESOURCE_NAME_REGIONI = "Farmacie.Services.Dati.Regioni.js";

        private List<Regione> ElencoRegioni;

        public IEnumerable<Regione> GetRegioni()
        {
            if (ElencoRegioni == null)
            {
                ElencoRegioni = new List<Regione>();

                var assembly = this.GetType().GetTypeInfo().Assembly;

                try
                {
                    using (Stream stream = assembly.GetManifestResourceStream(RESOURCE_NAME_REGIONI))
                    {
                        using (StreamReader re = new StreamReader(stream))
                        {
                            using (JsonTextReader reader = new JsonTextReader(re))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                ElencoRegioni = serializer.Deserialize<List<Regione>>(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Impossibile leggere le regioni", ex);
                }
            }

            return ElencoRegioni;
            
        }

        public Regione GetRegioneByCodice(string codice)
        {
            var regione = GetRegioni().SingleOrDefault(r => r.Codice == codice);
            return regione;
        }
    }
}
