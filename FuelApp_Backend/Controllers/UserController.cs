using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuelApp_Backend.Models;
using FuelApp_Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FuelApp_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Get All users
        [HttpGet]
        public JsonResult GetUsers()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var dbList = dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").AsQueryable();

            return new JsonResult(dbList);
        }

        //Create New User
        [HttpPost("create")]
        public JsonResult CreateUser(UserModel user)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            // Password Hash function call
            user.Password = PasswordHashing.Hash(user.Password);

            //Set Login status to false
            user.LoginStatus = false;

            dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").InsertOne(user);

            return new JsonResult("Inserted Successfully");
        }

        //Update User Information
        [HttpPut("update/{id}")]
        public JsonResult UpdateUser(UserModel user)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var filterId = Builders<UserModel>.Filter.Eq("Id", user.Id);

            var updateUser = Builders<UserModel>.Update.Set("Name", user.Name)
                .Set("Phone",user.Phone);

            dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").UpdateOne(filterId,updateUser);

            return new JsonResult("Updated Successfully");

        }

        //Update User Information
        [HttpDelete("{id}")]
        public JsonResult DeleteUser(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var filterId = Builders<UserModel>.Filter.Eq("Id", id);

            dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").DeleteOne(filterId);

            return new JsonResult("Deleted Successfully");

        }

        //User Login function
        [HttpPost("login")]
        public JsonResult Login(LoginModel login)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var dbList = dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").Find(user => user.Phone == login.Phone).ToList();

            // Verifying
            var result = PasswordHashing.Verify(login.Password, dbList[0].Password);

            if (result == true)
            {
                UserModel user = dbList[0];
                var filter = Builders<UserModel>.Filter.Eq("_id", user.Id);
                var update = Builders<UserModel>.Update.Set("LoginStatus", true);
                dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").UpdateOne(filter, update);
                var updateLogin = dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").Find(user => user.Phone == login.Phone).ToList();


                return new JsonResult(updateLogin);
            }
            else
            {
                return new JsonResult("Invalid User");
            }

        }
    }
}
