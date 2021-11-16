using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using project.api.Core.Interfaces;
using project.api.Data.Context;
using project.api.Data.Models;
using System.Linq;
using project.api.Data.DTO.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using project.api.Core.CustomExceptions;
using project.api.Core.Utilities;

namespace project.api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobServices _jobServices;
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private readonly Data.Models.User _user;

        public JobController(IJobServices jobServices, AppDBContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _jobServices = jobServices;
            _context = context;
            _mapper = mapper;
            _user = _context.Users.
                            First(u => u.UserName == httpContextAccessor.HttpContext.User.Identity.Name);
        }

        [HttpGet]
        [Authorize(Roles = "SimpleUser,Employer,Admin")]
        public IActionResult GetJobs()
        {
            var user = User;
            return Ok(_jobServices.GetJobs());
        }

        

        //[HttpGet]
        //[Authorize(Roles = "SimpleUser,Employer,Admin")]
        //[Route("~/User/{userId}/[controller]")]
        //public IActionResult GetJobsFromUser(string userId)
        //{
        //    return Ok(_jobServices.GetJobsFromUser(userId));
        //}

        [HttpGet("{id}", Name = "GetJob")]
        [Authorize(Roles = "SimpleUser,Employer,Admin")]
        public IActionResult GetJob(int id)
        {
            var CurJob = _jobServices.GetJob(id);
            if (CurJob == null)
            {
                return NotFound("Job Not Found");
            }
            return Ok(CurJob);
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpPost]
        public IActionResult CreateJob(CreateJobDTO job)
        {

            var jb = _mapper.Map<Job>(job);

            var newJob = _jobServices.CreateJob(jb);
            return CreatedAtAction("GetJob", new { userId = newJob.UserId, id = newJob.JobId }, newJob);
        }
        [Authorize(Roles = "Employer,Admin")]
        [HttpPut("{id}")]
        public IActionResult EditJob(int id, UpdateJobDTO job)
        {
            try
            {
                if (!_context.Jobs.Any(x => x.JobId == id))
                {
                    return NotFound("Job with that id doesnt exist");
                }
                var oldJob = _jobServices.GetJob(id);
                if (oldJob == null)
                    return NotFound("Job Not Found");
                _mapper.Map(job, oldJob);

                var edited = _jobServices.EditJob(oldJob);
                if (edited == true)
                {
              

                    var newJob = _jobServices.GetJob(id);
                    return Ok(newJob);
                }
                else
                {
                    return StatusCode(403, "User can not edit other users Job!");
                }
            }
            catch (JobException e)
            {
                return StatusCode(401, e.Message);
            }
        }
        [Authorize(Roles = "Employer,Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteJob(int id)
        {
            try
            {
                var job = _jobServices.GetJob(id);
                if (job == null)
                {
                    return NotFound("Job Not Found");
                }
                var deleted = _jobServices.DeleteJob(job);
                if (deleted == true)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(403, "User can not delete other users Job!");
                }

                }
            catch (JobException e)
            {
                return StatusCode(401, e.Message);
            }

        }
    }
}
