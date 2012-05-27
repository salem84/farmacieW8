using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacie.Models
{
    public class OgdiCollection<T>
    {
        public ICollection<T> d { get; set; }
    }
}
