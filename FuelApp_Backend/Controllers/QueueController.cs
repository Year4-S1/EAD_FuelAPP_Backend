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

        [HttpPost("add")]
        public JsonResult AddQueues(QueueModel queue)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            queue.Date = DateTime.Now.ToString("dd/MM/yyyy");
            queue.Time = DateTime.Now.ToString("HH:mm:ss");

            dbClient.GetDatabase("fueldb").GetCollection<QueueModel>("queue").InsertOne(queue);

            return new JsonResult("Added Successfully");
        }
    }
}
