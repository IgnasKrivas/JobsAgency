using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project.api.Core.Interfaces;
using project.api.Data.Context;
using project.api.Data.DTO.Skills;
using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    [ApiController]
    [Route("Application/{applicationId}/[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillServices _skillServices;
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        public SkillController(ISkillServices skillServices, AppDBContext context, IMapper mapper)
        {
            _skillServices = skillServices;
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet]
        public IActionResult GetSkills(int applicationId)
        {
            return Ok(_skillServices.GetSkills(applicationId)); 
        }

        [HttpGet("{id}", Name = "GetSkill")]
        public IActionResult GetSkill(int applicationId, int id)
        {
            var CurJob = _skillServices.GetSkill(applicationId, id);
            if (CurJob == null)
            {
                return NotFound("Skill Not Found");
            }
            return Ok(CurJob);
        }

        [HttpPost]
        public IActionResult CreateSkill(int applicationId,CreateSkillDTO skillDTO)
        {

            if (!_context.Applications.Any(x => x.ApplicationId == applicationId))
            {
                return NotFound("Application does not exists");
            }
            var skill = _mapper.Map<Skill>(skillDTO);
            skill.ApplicationId = applicationId;

            var newSkill = _skillServices.CreateSkill(skill);
            return CreatedAtAction("GetSkill", new { applicationId = applicationId, id = newSkill.SkillId }, newSkill);
        }
        [HttpPut("{id}")]
        public IActionResult EditSkill(int applicationId, int id, CreateSkillDTO skillDTO)
        {
            if (!_context.Skills.Any(x => x.SkillId == id))
            {
                return NotFound("Skill with that id doesnt exist");
            }
            var oldSkill = _skillServices.GetSkill(applicationId, id);
            if (oldSkill == null)
                return NotFound("Skill Not Found");
            _mapper.Map(skillDTO, oldSkill);

            _skillServices.EditSkill(oldSkill);
            var newSkill = _skillServices.GetSkill(applicationId, id);
            return Ok(newSkill);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteSkill(int applicationId, int id)
        {
            var skill = _skillServices.GetSkill(applicationId, id);
            if (skill == null)
            {
                return NotFound("Skill Not Found");
            }
            _skillServices.DeleteSkill(skill);

            return NoContent();

        }
    }
}
