using AutoMapper;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RequestUserMotorModel, User>();
            CreateMap<ResponseContractUserMotorModel, User>();
            CreateMap<User, RequestUserMotorModel>();
            CreateMap<User, ResponseContractUserMotorModel>();
            CreateMap<CnhModel, Cnh>();            
            CreateMap<Cnh, CnhModel>();            
            CreateMap<Cnh, ResponseCnhModel>();            
            CreateMap<ResponseCnhModel, Cnh>();            
            CreateMap<ContractPlanUserMotor, RequestContractPlanUserMotorModel>();
            CreateMap<ContractPlanUserMotor, ResponseContractUserFoorPlanModel>();
            CreateMap<ResponseContractUserFoorPlanModel, ContractPlanUserMotor>();
            CreateMap<RequestContractPlanUserMotorModel, ContractPlanUserMotor>();
            CreateMap<Plan, FoorPlanModel>();            
            CreateMap<MotorModel, MotorContractModel>();            
            CreateMap<ContractPlanUserMotor, ResponseContractUserFoorPlanModel>();            
        }
    }
}

