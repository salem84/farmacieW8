using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmacie.Models
{
    public class Regione : IEquatable<Regione>
    {
        public string Codice { get; set; }
        public string Nome { get; set; }

        public double Latitudine { get; set; }
        public double Longitudine { get; set; }

        //public Regione(string codice)
        //{
        //    Codice = codice;
        //}

        //TODO vedere come eliminare questa dipendenza da Json
        public Regione()
        {

        }

        //[JsonConstructor]
        public Regione(string codice, string nome)
        {
            Codice = codice;
            Nome = nome;
        }

        public override string ToString()
        {
            return string.Format("Regione: {0} ({1})", Nome, Codice);
        }

        #region EQUALITY OVERRIDE

        public static bool operator ==(Regione a, Regione b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(Regione a, Regione b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Regione r = obj as Regione;
            if ((System.Object)r == null)
            {
                return false;
            }

            return Equals(r);
        }

        public bool Equals(Regione r)
        {
            return Codice == r.Codice;
        }

        public override int GetHashCode()
        {
            return Codice.GetHashCode();
        }

        #endregion

    }
}
