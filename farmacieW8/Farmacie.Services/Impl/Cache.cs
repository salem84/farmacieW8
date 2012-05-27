using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Farmacie.Common.Utility.Expressions;

namespace Farmacie.Services.Impl
{
    public class Cache<T>
    {
        #region PRIVATE FIELDS
        
        private List<T> lista;
        private List<CacheEntry> queryEffettuate;

        #endregion

        // Funzione per prendere i valori
        public Func<Func<T, bool>, IEnumerable<T>> GetValues { get; set; }

        public Cache()
        {
            lista = new List<T>();
            queryEffettuate = new List<CacheEntry>();
        }


        public IQueryable<T> Where(Expression<Func<T, bool>> selector)
        {
            // estraggo le informazioni dall'espressione
            Type parametro = selector.Parameters.First().Type;
            //controllare logical binary expression
            Expression expr = ((BinaryExpression)selector.Body).Right;

            // mi creo l'entry per la cache
            CacheEntry entry = new CacheEntry(parametro, expr);

            var delegato = selector.Compile();

            if (!queryEffettuate.Contains(entry))
            {
                // non ho mai effettuato questa query
                var newValues = GetValues(delegato);
                
                // aggiungo i valori trovati
                Append(newValues);

                // aggiungo la query fatta
                queryEffettuate.Add(entry);

            }

            var result = lista.Where(delegato).AsQueryable<T>();
            return result;
        }

        protected void Append(IEnumerable<T> added)
        {
            lista.AddRange(added);
        }


        #region CACHE ENTRY
        
        class CacheEntry
        {
            Type field;
            Expression queryExpression;

            public CacheEntry(Type field, Expression queryExpresssion)
            {
                this.field = field;
                this.queryExpression = queryExpresssion;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                CacheEntry c = obj as CacheEntry;
                if ((System.Object)c == null)
                {
                    return false;
                }

                if(field != c.field)
                    return false;

                return ExpressionEqualityComparer.Instance.Equals(queryExpression, c.queryExpression);
            }

            public override int GetHashCode()
            {
                return field.GetHashCode() ^ new HashCodeCalculation(queryExpression).HashCode;
            }
        }

        #endregion

    }
}
