using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project.api.Core.Interfaces;
using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userServices.GetUsers()); 
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(int id)
        {
            return Ok(_userServices.GetUser(id));
        }

        [HttpPost]
        public IActionResult CreateJob(User user)
        {
            var newUser = _userServices.CreateUser(user);
            return CreatedAtRoute("GetUser",new { newUser.Id }, newUser);
        }
    }
}
