using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmacie.Models
{
    public class Farmacia
    {
        public Regione Regione 
        {
            get
            {
                return new Regione(codiceregione, descrizioneregione);
            }
            set
            {
                codiceregione = value.Codice;
                descrizioneregione = value.Nome;
            }
        }

        public string codiceregione { get; set; }
        public string descrizioneregione { get; set; }

        public string codiceidentificativofarmacia { get; set; }
        public string codfarmaciaassegnatodaasl { get; set; }
        public string partitaiva { get; set; }
        public string descrizionefarmacia { get; set; }
        


        public string indirizzo { get; set; }
        public string cap { get; set; }

        public string codicecomuneistat { get; set; }
        public string descrizionecomune { get; set; }
        
        public string frazione { get; set; }
        
        public string codiceprovinciaistat { get; set; }
        public string siglaprovincia { get; set; }
        public string descrizioneprovincia { get; set; }

        public string latitudine { get; set; }
        public string longitudine { get; set; }
    }
}
