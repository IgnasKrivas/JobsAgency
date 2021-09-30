using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.api.Core.Interfaces
{
    public interface ISkillServices
    {
        List<Skill> GetSkills();
        List<Skill> GetSkillsFromApplication(int applicationId);
        Skill GetSkill(int id);
        Skill CreateSkill(Skill skill);
        void EditSkill(Skill skill);
        void DeleteSkill(Skill skill);

    }
}
