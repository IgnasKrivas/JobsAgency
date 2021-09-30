using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using project.api.Core.Interfaces;
using project.api.Data.Context;
using project.api.Data.Models;
using System.Linq;
using project.api.Data.DTO.Jobs;

namespace project.api.Controllers
{
    [ApiController]
    [Route("User/{userId}/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobServices _jobServices;
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public JobController(IJobServices jobServices, AppDBContext context, IMapper mapper)
        {
            _jobServices = jobServices;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetJobs(int userId)
        {
            return Ok(_jobServices.GetJobs(userId));
        }

        [HttpGet("{id}", Name = "GetJob")]
        public IActionResult GetJob(int userId, int id)
        {
            var CurJob = _jobServices.GetJob(userId, id);
            if (CurJob == null)
            {
                return NotFound("Job Not Found");
            }
            return Ok(CurJob);
        }

        [HttpPost]
        public IActionResult CreateJob(int userId, CreateJobDTO job)
        {
            if (!_context.Users.Any(x => x.Id == userId))
            {
                return NotFound("User is not exists");
            }

            var jb = _mapper.Map<Job>(job);
            jb.UserId = userId;

            var newJob = _jobServices.CreateJob(jb);
            return CreatedAtAction("GetJob", new { userId = userId, id = newJob.JobId }, newJob);
        }
        [HttpPut("{id}")]
        public IActionResult EditJob(int userId, int id, CreateJobDTO job)
        {
            if (!_context.Jobs.Any(x => x.JobId == id))
            {
                return NotFound("Job with that id doesnt exist");
            }
            var oldJob = _jobServices.GetJob(userId, id);
            if (oldJob == null)
                return NotFound("Job Not Found");
            _mapper.Map(job, oldJob);

            _jobServices.EditJob(oldJob);
            var newJob = _jobServices.GetJob(userId, id);
            return Ok(newJob);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteApplication(int userId, int id)
        {
            var job = _jobServices.GetJob(userId, id);
            if (job == null)
            {
                return NotFound("Job Not Found");
            }
            _jobServices.DeleteJob(job);

            return NoContent();

        }
    }
}
