﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using project.api.Data.DTO.Applications;
using project.api.Data.DTO.Jobs;
using project.api.Data.DTO.Skills;
using project.api.Data.Models;

namespace project.api.Data.DTO
{
    public class projectProfile : Profile
    {
        public projectProfile()
        {
            CreateMap<CreateApplicationDTO, Application>();
            CreateMap<UpdateApplicationDTO, Application>();

            CreateMap<CreateJobDTO, Job>();
            CreateMap<CreateSkillDTO, Skill>();
        }
    }
}
