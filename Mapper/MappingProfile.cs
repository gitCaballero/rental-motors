﻿using AutoMapper;
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
            CreateMap<RequestUserMotorModel, UserMotor>();
            CreateMap<ResponseContractUserMotorModel, UserMotor>();
            CreateMap<UserMotor, RequestUserMotorModel>();
            CreateMap<UserMotor, ResponseContractUserMotorModel>();
            CreateMap<CnhModel, Cnh>();            
            CreateMap<Cnh, CnhModel>();            
            CreateMap<ContractUserFoorPlan, RequestContractUserFoorPlanModel>();
            CreateMap<ContractUserFoorPlan, ResponseContractUserFoorPlanModel>();
            CreateMap<ResponseContractUserFoorPlanModel, ContractUserFoorPlan>();
            CreateMap<RequestContractUserFoorPlanModel, ContractUserFoorPlan>();
            CreateMap<FoorPlan, FoorPlanModel>();            
            CreateMap<ResponseMotorModel, MotorModelContract>();            
        }
    }
}

