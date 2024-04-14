using AutoMapper;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;

namespace RentalMotor.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserMotorModel, UserMotor>();
            CreateMap<CnhModel, Cnh>().ForMember(dest => dest.CnhCategories, opt => opt.MapFrom(src => src.CnhCategories.Select(x => ((int)x))));            
            CreateMap<FoorPlan, FoorPlanModel>();
        }
    }
}

