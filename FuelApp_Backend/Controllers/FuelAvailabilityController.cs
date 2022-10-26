using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuelApp_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FuelApp_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelAvailabilityController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FuelAvailabilityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //View Fuel Availability Details
        [HttpGet]
        public JsonResult GetFeulAvailability()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var dbList = dbClient.GetDatabase("fueldb").GetCollection<FuelAvailabilityModel>("fuelavailability").AsQueryable();

            return new JsonResult(dbList);
        }


        //Add Fuel Availability data
        [HttpPost("add")]
        public JsonResult AddFuelAvailability(FuelAvailabilityModel fuelInfo)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            dbClient.GetDatabase("fueldb").GetCollection<FuelAvailabilityModel>("fuelavailability").InsertOne(fuelInfo);

            return new JsonResult("Inserted Successfully");
        }


        //Get fuel availability per station
        [HttpGet("getstation/{id}")]
        public JsonResult GetFuelAvailabilityForOneStation(string Id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var Station_fuel_list = dbClient.GetDatabase("fueldb").GetCollection<FuelAvailabilityModel>("fuelavailability").Find(fueldetails => fueldetails.StationID == Id).ToList();

            return new JsonResult(Station_fuel_list[0]);
        }

        [HttpPut("update/availability/{id}")]
        public JsonResult UpdateFuelAvailability (string id, FuelAvailabilityModel fuelAvailability)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var fuelId = new ObjectId(id);
            var filter = Builders<FuelAvailabilityModel>.Filter.Eq("_id", fuelId);
            var update = Builders<FuelAvailabilityModel>.Update.Set("Availability", fuelAvailability.Availability).Set("AmountOfFuel", fuelAvailability.AmountOfFuel);
            dbClient.GetDatabase("fueldb").GetCollection<FuelAvailabilityModel>("fuelavailability").UpdateOne(filter, update);
            var updated_fuel_availability = dbClient.GetDatabase("fueldb").GetCollection<FuelAvailabilityModel>("fuelavailability").Find(fuel => fuel.Id == fuelId).ToList();

            return new JsonResult(updated_fuel_availability[0]);
        }
    }
}
