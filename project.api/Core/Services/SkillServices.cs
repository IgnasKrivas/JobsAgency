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
        public SkillServices(AppDBContext context)
        {
            _context = context;
        }

        public Skill CreateSkill(Skill skill)
        {
            _context.Add(skill);
            _context.SaveChanges();

            return skill;
        }

        public void DeleteSkill(Skill skill)
        {
            _context.Skills.Remove(skill);
            _context.SaveChanges();
        }

        public void EditSkill(Skill skill)
        {
            _context.Skills.Update(skill);
            _context.SaveChanges();
        }

        public Skill GetSkill(int applicationId, int id)
        {
            return _context.Skills.Where(x => x.SkillId == id
                                        && x.Application.ApplicationId == applicationId)
                                .SingleOrDefault();
        }

        public List<Skill> GetSkills(int applicationId)
        {
            return _context.Skills.Where(x => x.Application.ApplicationId == applicationId)
                                        .ToList();
           
        }
    }
}
