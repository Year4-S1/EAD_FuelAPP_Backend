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
    public class StationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //View all Stations
        [HttpGet("get/all")]
        public JsonResult GetStations()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var dbList = dbClient.GetDatabase("fueldb").GetCollection<StationModel>("station").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpGet("{id:int}")]
        public JsonResult GetStationByID(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var result = dbClient.GetDatabase("fueldb").GetCollection<StationModel>("station").AsQueryable();

            return new JsonResult(result);
        }    

        //Add stations
        [HttpPost("create")]
        public JsonResult AddStations(StationModel station)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            dbClient.GetDatabase("fueldb").GetCollection<StationModel>("station").InsertOne(station);

            return new JsonResult(station);
        }


        //Search stations
        [HttpGet("search/{station}")]
        public JsonResult SearchStation(string station)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var stationValue = station;

            //check if the station name is not null
            if (station != null)
            {
                var dbList = dbClient.GetDatabase("fueldb").GetCollection<StationModel>("station").Find(station => station.StationName.ToLower() == stationValue.ToLower() || station.District.ToLower() == stationValue.ToLower()).ToList();

                return new JsonResult(dbList);
            }
            else
            {
                return new JsonResult("Please Enter a value Search");
            }
        }

        [HttpPut("update/availability/{id}")]
        public JsonResult UpdateFuelAvailability(string id, StationModel fuelAvailability)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var fuelId = new ObjectId(id);
            //filter by fuelId
            var filter = Builders<StationModel>.Filter.Eq("_id", fuelId);
            //update fuel status and amount
            var update = Builders<StationModel>.Update.Set("Availability", fuelAvailability.Availability).Set("AmountOfFuel", fuelAvailability.AmountOfFuel);
            dbClient.GetDatabase("fueldb").GetCollection<StationModel>("station").UpdateOne(filter, update);
            var updated_fuel = dbClient.GetDatabase("fueldb").GetCollection<StationModel>("station").Find(fueldetail => fueldetail.Id == fuelId).ToList();

            return new JsonResult(updated_fuel[0]);
        }


        [HttpGet("getstation/{id}")]
        public JsonResult UpdateStationDetails(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var ownerId = id;

            var dbList = dbClient.GetDatabase("fueldb").GetCollection<StationModel>("station").Find(station => station.StationOwnerID.ToLower() == ownerId.ToLower()).ToList();

            return new JsonResult(dbList[0]);

        }
    }
}
