using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuelApp_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        [HttpGet]
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

            return new JsonResult("Inserted Successfully");
        }

        //Search Function
        //public async Task<AcceptedResult<Enumerable<StationModel>>> SearchStation(string location)
        //{

        //}

    }
}
