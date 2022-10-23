using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace FuelApp_Backend.Models
{
    public class QueueModel
    {
        public ObjectId Id { get; set; }

        public string Date { get; set; }

        public string ArrivalTime { get; set; }

        public string DepartureTime { get; set; }

        public string StationId { get; set; }
        
        public string Status { get; set; } //Joined or Not

        public string CustomerID { get; set; }

        public string VehicleType { get; set; }

        public string FuelType { get; set; }
    }
}
