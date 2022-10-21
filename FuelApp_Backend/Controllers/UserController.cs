﻿using System;
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
        public JsonResult PostUsers(UserModel user)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            // Password Hash function call
            user.Password = PasswordHashing.Hash(user.Password);

            dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").InsertOne(user);

            return new JsonResult("Inserted Successfully");
        }

        //Update User Information
        [HttpPut("update/{id}")]
        public JsonResult UpdateUser(UserModel user)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApplication"));

            var filterId = Builders<UserModel>.Filter.Eq("Id", user.Id);

            var updateUser = Builders<UserModel>.Update.Set("Phone", user.Phone);

            dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").UpdateOne(filterId,updateUser);

            return new JsonResult("Update Successfully");

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
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("FuelApp"));


            var dbList = dbClient.GetDatabase("fueldb").GetCollection<UserModel>("user").Find(user => user.Phone == login.Phone).ToList();

            // Verify
            var result = PasswordHashing.Verify(login.Password, dbList[0].Password);

            if (result == true)
            {
                return new JsonResult("Valid User");
            }
            else
            {
                return new JsonResult("Invalid User");
            }

        }
    }
}