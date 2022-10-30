using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelApp_Backend.Models
{
    public class FuelTypes
    {
        public string PETROL92 { get; set; }
        
        public string PETROL95 { get; set; }

        public string DIESEL { get; set; }

        public string SUPERDIESEL { get; set; }

        public FuelTypes(string fuel)
        {
        }

        public FuelTypes()
        {
        }
    }
}
