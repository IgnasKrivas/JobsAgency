using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project.api.Core.Interfaces;
using project.api.Data.Context;
using project.api.Data.DTO.Applications;
using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationServices _applicationServices;
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        public ApplicationController(IApplicationServices applicationServices, AppDBContext context, IMapper mapper)
        {
            _applicationServices = applicationServices;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("~/Job/{jobId}/[controller]")]
        public IActionResult GetApplicationsFromJob(int jobId)
        {         
            return Ok(_applicationServices.GetApplicationsFromJob(jobId));
        }

        [HttpGet]
        [Route("~/User/{userId}/[controller]")]
        public IActionResult GetApplicationsFromUser(int userId)
        {
            return Ok(_applicationServices.GetApplicationsFromUser(userId));
        }
        [HttpGet]
        public IActionResult GetApplications()
        {
            return Ok(_applicationServices.GetApplications());
        }

        [HttpGet("{id}", Name = "GetApplication")]
        public IActionResult GetApplication(int id)
        {
            var CurApplication = _applicationServices.GetApplication(id);
            if (CurApplication == null)
            {
                return NotFound("Application Not Found");
            }
            return Ok(CurApplication);
        }

        [HttpPost]
        public IActionResult CreateApplication(CreateApplicationDTO application)
        {
            if (!_context.Users.Any(x => x.Id == application.CandidateId))
            {
                return NotFound("User does not exists");
            }
            if (!_context.Jobs.Any(x => x.JobId == application.JobId))
            {
                return NotFound("Job does not exists");
            }

            var app = _mapper.Map<Application>(application);

            var newApplication = _applicationServices.CreateApplication(app);
            return CreatedAtAction("GetApplication", new { jobId = newApplication.JobId , id = newApplication.ApplicationId},newApplication);
        }

        [HttpPut("{id}")]
        public IActionResult EditApplication(int id, UpdateApplicationDTO application)
        {
            if (!_context.Applications.Any(x => x.ApplicationId == id))
            {
                return NotFound("Application with that id doesnt exist");
            }
            var oldApp = _applicationServices.GetApplication(id);
            if (oldApp == null)
                return NotFound();
            _mapper.Map(application, oldApp);

            _applicationServices.EditApplication(oldApp);
            var newApp = _applicationServices.GetApplication(id);
            return Ok(newApp);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteApplication(int id)
        {
            var app = _applicationServices.GetApplication(id);
            if(app == null)
            {
                return NotFound("Application Not Found");
            }
            _applicationServices.DeleteApplication(app);

            return NoContent();

        }
        
    }
}
