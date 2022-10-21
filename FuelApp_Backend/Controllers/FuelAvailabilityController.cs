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
    }
}
