using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project.api.Core.CustomExceptions;
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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
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
        [Route("~/Application/{applicationId}/[controller]")]
        public IActionResult GetSkillsFromApplication(int applicationId)
        {
            return Ok(_skillServices.GetSkillsFromApplication(applicationId)); 
        }

        [HttpGet]
        public IActionResult GetSkills()
        {
            return Ok(_skillServices.GetSkills());
        }

        [HttpGet("{id}", Name = "GetSkill")]
        public IActionResult GetSkill(int id)
        {
            var CurJob = _skillServices.GetSkill(id);
            if (CurJob == null)
            {
                return NotFound("Skill Not Found");
            }
            return Ok(CurJob);
        }

        [HttpPost]
        public IActionResult CreateSkill(CreateSkillDTO skillDTO)
        {

            if (!_context.Applications.Any(x => x.ApplicationId == skillDTO.ApplicationId))
            {
                return NotFound("Application does not exists");
            }
            var skill = _mapper.Map<Skill>(skillDTO);

            var newSkill = _skillServices.CreateSkill(skill);
            return CreatedAtAction("GetSkill", new { applicationId = newSkill.ApplicationId, id = newSkill.SkillId }, newSkill);
        }
        [HttpPut("{id}")]
        public IActionResult EditSkill(int id, UpdateSkillDTO skillDTO)
        {
            try
            {
                if (!_context.Skills.Any(x => x.SkillId == id))
                {
                    return NotFound("Skill with that id doesnt exist");
                }
                var oldSkill = _skillServices.GetSkill(id);
                if (oldSkill == null)
                    return NotFound("Skill Not Found");
                _mapper.Map(skillDTO, oldSkill);

                var edited = _skillServices.EditSkill(oldSkill);
                if (edited == true)
                {


                    var newSkill = _skillServices.GetSkill(id);
                    return Ok(newSkill);
                }
                else
                {
                    return StatusCode(403, "User can not edit other users Skills!");
                }
            }
            catch(SkillException e)
            {
                return StatusCode(401, e.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteSkill(int id)
        {
            try
            {
                var skill = _skillServices.GetSkill(id);
                if (skill == null)
                {
                    return NotFound("Skill Not Found");
                }
                var deleted = _skillServices.DeleteSkill(skill);
                if (deleted == true)
                {


                    return NoContent();
                }
                else
                {
                    return StatusCode(403, "User can not delete other users Skills!");
                }
            }
            catch(SkillException e)
            {
                return StatusCode(401, e.Message);
            }

        }
    }
}
