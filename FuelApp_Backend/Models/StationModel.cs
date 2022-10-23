using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace FuelApp_Backend.Models
{
    public class StationModel
    {
        public ObjectId Id { get; set; }

        public string StationName { get; set; }

        public string Location { get; set; }

        public string District { get; set; }
    }
}
