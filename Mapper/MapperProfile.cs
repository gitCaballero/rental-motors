using AutoMapper;
using RentalMotor.Api.Complement.Enums;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;

namespace RentalMotor.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserMotorModel, UserMotor>();
                /*.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Cnh, opt => opt.MapFrom(src => src.Cnh))
                .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.CpfCnpj));*/
            // Adicione outras configurações de mapeamento conforme necessário

            CreateMap<CnhModel, Cnh>().ForMember(dest => dest.CnhCategories, opt => opt.MapFrom(src => src.CnhCategories.Select(x => ((int)x))));            
            CreateMap<FoorPlan, FoorPlanModel>();
        }
    }
}

