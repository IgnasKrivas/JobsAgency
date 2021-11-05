using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project.api.Core.CustomExceptions;
using project.api.Core.Interfaces;
using project.api.Data.DTO.Users;
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
        private readonly IMapper _mapper;
        public UserController(IUserServices userServices, IMapper mapper)
        {
            _userServices = userServices;
            _mapper = mapper;
        }
        
        [HttpPost("signup")]
        public IActionResult SignUp(User user)
        {
            try
            {
                var result = _userServices.SignUp(user);
                return Created("", result);
            }
            catch(UsernameAlreadyExistsException e)
            {
                return StatusCode(409,e.Message);
            }
        }
        [HttpPost("signin")]
        public IActionResult SignIn(SignInUser userDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userDTO);
                var result = _userServices.SignIn(user);
                return Ok(result);
            }
            catch(InvalidUsernamePasswordException e)
            {
                return StatusCode(401, e.Message);
            }
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
