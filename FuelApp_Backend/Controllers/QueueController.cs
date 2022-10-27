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

            return new JsonResult(queue);
        }


        //Update departure time 
        [HttpPut("update/depart/{id}")]
        public JsonResult UpdateDepartureTime(String id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var queueID = new ObjectId(id);
            var filterId = Builders<QueueModel>.Filter.Eq("_id", queueID);
            var updateTime = Builders<QueueModel>.Update.Set("DepartureTime", DateTime.Now.ToString("HH:mm:ss"));

            dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").UpdateOne(filterId, updateTime);

            var departTime = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").Find(queueTime => queueTime.Id == queueID).ToList();

            return new JsonResult(departTime[0]);
        }


        [HttpGet("count/vehicle/type/{id}")]
        public JsonResult GetQueueLenghtByVehicleType(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            //counting the vehicles in the queue per station
            int QueueCarCount = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "car".ToLower() && queue.Status.ToLower() == "Joined".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueMotorCycleCount = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "motorCycle".ToLower() && queue.Status.ToLower() == "Joined".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueThreeWheelersCount = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "threeWheelers".ToLower() && queue.Status.ToLower() == "Joined".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueVanCount = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "van".ToLower() && queue.Status.ToLower() == "Joined".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueLorryCount = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "lorry".ToLower() && queue.Status.ToLower() == "Joined".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));
            int QueueBusCount = dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").AsQueryable().Count(queue => queue.StationId == id && queue.VehicleType.ToLower() == "bus".ToLower() && queue.Status.ToLower() == "Joined".ToLower() && queue.Date == DateTime.Now.ToString("dd/MM/yyyy"));

            int TotalQueueLength = QueueCarCount + QueueMotorCycleCount + QueueThreeWheelersCount + QueueVanCount + QueueLorryCount + QueueBusCount;

            //adding the vehicle counts to the dictionary
            Dictionary<string, int> QueueVehicleCountN = new Dictionary<string, int>();
            QueueVehicleCountN.Add("Car", QueueCarCount);
            QueueVehicleCountN.Add("MotorCycle", QueueMotorCycleCount);
            QueueVehicleCountN.Add("ThreeWheelers", QueueThreeWheelersCount);
            QueueVehicleCountN.Add("Van", QueueVanCount);
            QueueVehicleCountN.Add("Lorry", QueueLorryCount);
            QueueVehicleCountN.Add("Bus", QueueBusCount);
            QueueVehicleCountN.Add("TotalQueueLength", TotalQueueLength);

            return new JsonResult(QueueVehicleCountN);
        }
    }
}
