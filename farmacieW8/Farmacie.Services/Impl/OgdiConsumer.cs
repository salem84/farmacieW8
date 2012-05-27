using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Farmacie.Models;

namespace Farmacie.Services.Impl
{
    public class OgdiConsumer
    {
        private string _nextPartitionKey = null;
        private string _nextRowKey = null;

        /// <summary>
        /// Url to the data set that we want to consume
        /// </ Summary>
        public string DatasetUrl { get; set; }

        /// <summary>
        /// Filter OData that is to be applied on the dataset
        /// </ Summary>
        public string QueryFilter { get; set; }

        /// <summary>
        /// Method for a data set consomer OGDI so paged
        /// </ Summary>
        /// <typeparam Name="T"> type objects "proxy" to return </ paramType>
        /// <param Name="count"> Number of objects to return </ param>
        /// <param Name="collection"> collection of objects to populate </ param>
        /// <returns> An asynchronous operation returns true   if the dataset
        /// still contains entities, false otherwise. </ returns>
        public async Task<bool> LoadNextDataChunkAsync<T>(int count, ICollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            // Construction de la requête
            StringBuilder oDataRequestBuilder = new StringBuilder(DatasetUrl);
            oDataRequestBuilder.Append("/?");

            if (!string.IsNullOrEmpty(QueryFilter))
            {
                oDataRequestBuilder.Append("$filter=");
                oDataRequestBuilder.Append(QueryFilter);
                oDataRequestBuilder.Append("&");
            }

            oDataRequestBuilder.Append("$top=");
            oDataRequestBuilder.Append(count);

            if (_nextPartitionKey != null && _nextRowKey != null)
            {
                oDataRequestBuilder.Append("&NextPartitionKey=");
                oDataRequestBuilder.Append(_nextPartitionKey);
                oDataRequestBuilder.Append("&NextRowKey=");
                oDataRequestBuilder.Append(_nextRowKey);
            }
            oDataRequestBuilder.Append("&format=json");

            // Exécution de la requête
            HttpClient client = new HttpClient();
            HttpResponseMessage message = await client.GetAsync(oDataRequestBuilder.ToString());

            if (!message.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Response code from " +
                          oDataRequestBuilder.ToString() +
                          " : " + message.StatusCode);
            }

            // Analyse de la réponse (recherche des jetons de continuation de la table Azure)
            IEnumerable<string> continuationNextPartitionKeyHeader = null;
            IEnumerable<string> continuationNextRowKeyHeader = null;
            bool result = message.Headers.TryGetValues("x-ms-continuation-NextPartitionKey",
                  out continuationNextPartitionKeyHeader);
            result = message.Headers.TryGetValues("x-ms-continuation-NextRowKey",
                  out continuationNextRowKeyHeader);

            if (result == true)
            {
                _nextPartitionKey = continuationNextPartitionKeyHeader.First();
                _nextRowKey = continuationNextRowKeyHeader.First();
            }
            else
            {
                _nextPartitionKey = null;
                _nextRowKey = null;
            }

            // Désérialisation des objets en JSON
            string jsonContent = await message.Content.ReadAsStringAsync();
            OgdiCollection<T> tmpCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<OgdiCollection<T>>(jsonContent);

            // Ajout des objets désérialisés à la collection passée en paramètre
            // collection = tmpCollection.d;
            foreach (var item in tmpCollection.d)
            {
                collection.Add(item);
            }

            if (_nextPartitionKey == null && _nextRowKey == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public ICollection<T> LoadNextDataChunk<T>(int count)
        {
            ICollection<T> collection = new List<T>();

            // Construction de la requête
            StringBuilder oDataRequestBuilder = new StringBuilder(DatasetUrl);
            oDataRequestBuilder.Append("/?");

            if (!string.IsNullOrEmpty(QueryFilter))
            {
                oDataRequestBuilder.Append("$filter=");
                oDataRequestBuilder.Append(QueryFilter);
                oDataRequestBuilder.Append("&");
            }

            oDataRequestBuilder.Append("$top=");
            oDataRequestBuilder.Append(count);

            if (_nextPartitionKey != null && _nextRowKey != null)
            {
                oDataRequestBuilder.Append("&NextPartitionKey=");
                oDataRequestBuilder.Append(_nextPartitionKey);
                oDataRequestBuilder.Append("&NextRowKey=");
                oDataRequestBuilder.Append(_nextRowKey);
            }
            oDataRequestBuilder.Append("&format=json");

            // Exécution de la requête
            HttpClient client = new HttpClient();
            HttpResponseMessage message = client.GetAsync(oDataRequestBuilder.ToString()).Result;

            if (!message.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Response code from " +
                          oDataRequestBuilder.ToString() +
                          " : " + message.StatusCode);
            }

            // Analyse de la réponse (recherche des jetons de continuation de la table Azure)
            IEnumerable<string> continuationNextPartitionKeyHeader = null;
            IEnumerable<string> continuationNextRowKeyHeader = null;
            bool result = message.Headers.TryGetValues("x-ms-continuation-NextPartitionKey",
                  out continuationNextPartitionKeyHeader);
            result = message.Headers.TryGetValues("x-ms-continuation-NextRowKey",
                  out continuationNextRowKeyHeader);

            if (result == true)
            {
                _nextPartitionKey = continuationNextPartitionKeyHeader.First();
                _nextRowKey = continuationNextRowKeyHeader.First();
            }
            else
            {
                _nextPartitionKey = null;
                _nextRowKey = null;
            }

            // Désérialisation des objets en JSON
            string jsonContent = message.Content.ReadAsStringAsync().Result;
            OgdiCollection<T> tmpCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<OgdiCollection<T>>(jsonContent);

            // Ajout des objets désérialisés à la collection passée en paramètre
            // collection = tmpCollection.d;
            foreach (var item in tmpCollection.d)
            {
                collection.Add(item);
            }

            //if (_nextPartitionKey == null && _nextRowKey == null)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}

            return collection;
        }

        /// <Summary>
        /// Method to reset pagination
        /// </ Summary>
        public void ResetPagination()
        {
            _nextPartitionKey = null;
            _nextRowKey = null;
        }

    }
}
    

