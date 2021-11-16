using Microsoft.AspNetCore.Http;
using project.api.Core.Interfaces;
using project.api.Data.Context;
using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project.api.Core.Services
{
    public class SkillServices : ISkillServices
    {
        private readonly AppDBContext _context;
        private readonly User _user;
        public SkillServices(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = _context.Users
                       .First(u => u.UserName == httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public Skill CreateSkill(Skill skill)
        {
            var app = _context.Applications.Where(a => a.ApplicationId == skill.ApplicationId).First();
            if (app.CandidateId == _user.Id)
            {
                _context.Add(skill);
                _context.SaveChanges();
                // return skill;
            }
            return skill;
            //return skill;
        }

        public bool DeleteSkill(Skill skill)
        {
            var dbSkill = _context.Skills.Where(s => s.Application.CandidateId == _user.Id && s.SkillId == skill.SkillId).FirstOrDefault();
            if (dbSkill != null)
            {
                _context.Skills.Remove(skill);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditSkill(Skill skill)
        {
            var dbSkill = _context.Skills.Where(s => s.Application.CandidateId == _user.Id && s.SkillId == skill.SkillId).FirstOrDefault();
            if (dbSkill != null)
            {
                _context.Skills.Update(skill);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Skill GetSkill(int id)
        {
            return _context.Skills.Where(x => x.SkillId == id)
                                  .SingleOrDefault();
        }

        //public List<Skill> GetSkills()
        //{
        //    throw new NotImplementedException();
        //}

        public List<Skill> GetSkills()
        {
            return _context.Skills.Where(s => s.Application.CandidateId == _user.Id).ToList();
        }

        public List<Skill> GetSkillsFromApplication(int applicationId)
        {
            return _context.Skills.Where(x => x.Application.ApplicationId == applicationId)
                                        .ToList();
        }
    }
}
