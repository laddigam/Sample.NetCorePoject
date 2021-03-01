using AutoMapper;
using Joho.Services.DTO.Response;
using Joho.Services.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joho.Services.Config
{
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
          
            CreateMap<UserInfoReq, UserInfoRes>().ReverseMap();
            CreateMap<SalaryInfoReq, SalaryInfoRes>().ReverseMap();
        }
    }
}
