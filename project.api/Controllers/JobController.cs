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
    [Route("[controller]")]
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
        public IActionResult GetJobs()
        {
            return Ok(_jobServices.GetJobs());
        }

        [HttpGet]
        [Route("~/User/{userId}/[controller]")]
        public IActionResult GetJobsFromUser(int userId)
        {
            return Ok(_jobServices.GetJobsFromUser(userId));
        }

        [HttpGet("{id}", Name = "GetJob")]
        public IActionResult GetJob(int id)
        {
            var CurJob = _jobServices.GetJob(id);
            if (CurJob == null)
            {
                return NotFound("Job Not Found");
            }
            return Ok(CurJob);
        }

        [HttpPost]
        public IActionResult CreateJob(CreateJobDTO job)
        {
            if (!_context.Users.Any(x => x.Id == job.UserId))
            {
                return NotFound("User is not exists");
            }

            var jb = _mapper.Map<Job>(job);

            var newJob = _jobServices.CreateJob(jb);
            return CreatedAtAction("GetJob", new { userId = newJob.UserId, id = newJob.JobId }, newJob);
        }
        [HttpPut("{id}")]
        public IActionResult EditJob(int id, UpdateJobDTO job)
        {
            if (!_context.Jobs.Any(x => x.JobId == id))
            {
                return NotFound("Job with that id doesnt exist");
            }
            var oldJob = _jobServices.GetJob(id);
            if (oldJob == null)
                return NotFound("Job Not Found");
            _mapper.Map(job, oldJob);

            _jobServices.EditJob(oldJob);
            var newJob = _jobServices.GetJob(id);
            return Ok(newJob);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteApplication(int id)
        {
            var job = _jobServices.GetJob(id);
            if (job == null)
            {
                return NotFound("Job Not Found");
            }
            _jobServices.DeleteJob(job);

            return NoContent();

        }
    }
}
