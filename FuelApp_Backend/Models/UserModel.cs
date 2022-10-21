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

<<<<<<< HEAD
        public bool LoginStatus { get; set; }
=======
        public Boolean LoginStatus { get; set; }
>>>>>>> 7ebe3b0a30dd1f8e32daf33a7df7aa50a56ac5d8

    }
}
