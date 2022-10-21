using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace FuelApp_Backend.Models
{
    public class UserModel
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string UserType { get; set; }

        public Boolean LoginStatus { get; set; }

    }
}
