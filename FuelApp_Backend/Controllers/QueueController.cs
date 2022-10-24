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
    public class QueueController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public QueueController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       
        //View Queue Details
        [HttpGet]
        public JsonResult GetQueues()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var dbList = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable();

            return new JsonResult(dbList);
        }

        //Add queue details
        [HttpPost("add")]
        public JsonResult AddQueues(QueueModel queue)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            queue.Date = DateTime.Now.ToString("dd/MM/yyyy");
            queue.ArrivalTime = DateTime.Now.ToString("HH:mm:ss");
            queue.DepartureTime = "";

            dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").InsertOne(queue);

            return new JsonResult("Added Successfully");
        }


        //Update departure time 
        [HttpPut("update/depart/{id}")]
        public JsonResult UpdateTime(String id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var queueID = new ObjectId(id);
            var filterId = Builders<QueueModel>.Filter.Eq("_id", queueID);
            var updateTime = Builders<QueueModel>.Update.Set("DepartureTime", DateTime.Now.ToString("HH:mm:ss"));

            dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").UpdateOne(filterId, updateTime);

            var departTime = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").Find(queueTime => queueTime.Id == queueID).ToList();

            return new JsonResult("Updated Successfully");
        }


        //Get Queue length by vehicle type
        [HttpGet("total/vehicletype/{id}")]
        public JsonResult GetQueueLenght(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            //Count the vehicles in the queue in a station
            int amountOfCars = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "car".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int amountOfVans = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "van".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int amountOfLorries = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "lorry".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int amountOfBuses = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "bus".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int amountOfMotorBikes = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "motor cycle".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int amountOfThreeWheelers = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "three-wheelers".ToLower() && queue.Status.ToLower() == "Exit".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));

            //Adding the vehicle total count to a dictionary
            Dictionary<string, int> QueueCount = new Dictionary<string, int>();
            QueueCount.Add("Car", amountOfCars);
            QueueCount.Add("Van", amountOfVans);
            QueueCount.Add("Lorry", amountOfLorries);
            QueueCount.Add("Bus", amountOfBuses);
            QueueCount.Add("MotorCycle", amountOfMotorBikes);
            QueueCount.Add("Three-Wheelers", amountOfThreeWheelers);

            return new JsonResult(QueueCount);
        }
    }
}
