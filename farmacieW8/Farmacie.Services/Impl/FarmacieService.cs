using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmacie.Models;

namespace Farmacie.Services.Impl
{
    public class FarmacieService : IFarmacieService
    {
//        private List<Regione> regioniScaricate;
        private Cache<Farmacia> cache;
        private OgdiConsumer ogdiConsumer;

        public FarmacieService(Cache<Farmacia> c)
        {
            //regioniScaricate = new List<Regione>();
            ogdiConsumer = new OgdiConsumer()
            {
                DatasetUrl = "http://opendatasalutedata.cloudapp.net/v1/datacatalog/Farmacie/"
            };

            this.cache = c;
        }

        public IEnumerable<Farmacia> GetAllFarmacie()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Farmacia> GetFarmaciaByRegione(Regione regione)
        {
            ogdiConsumer.QueryFilter = string.Format("codiceregione eq '{0}'", regione.Codice);

            cache.GetValues = (f =>
                {
                    var result = ogdiConsumer.LoadNextDataChunk<Farmacia>(10);

                    return result;
                });


            var r = cache.Where(q => q.Regione == regione);
            return r;

            /*Task t = new Task(new Action(() =>
            {
                bool loadMore = true;

                while (ogdiConsumer.LoadNextDataChunk(5, cache.).Result == true && loadMore == true)
                {
                    
                }
            }));

            t.Start();
            t.Wait();*/




            /*if (!regioniScaricate.Contains(regione))
            {

            }

            var r = cache.Where(f => f.Regione == regione);

            var result = from f in cache
                         where f.Regione == regione
                         select f;

            return result;*/

            //throw new NotImplementedException();
        }
    }
}
