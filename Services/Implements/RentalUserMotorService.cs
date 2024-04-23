﻿using AutoMapper;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Services.Interfaces;
using RentalMotor.Api.Services.Network;
using RentalMotor.Api.Services.Responsabilities;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RentalMotor.Api.Services.Implements
{
    public class RentalUserMotorService : IRentalUserMotorService
    {
        public readonly IUserMotorRepository _userMotorRepository;
        public readonly IFoorPlanService _foorPlanService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMotorService _motorService;
        private string UserId;
        private string UserName;

        public RentalUserMotorService(IUserMotorRepository userMotorRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IFoorPlanService foorPlanService, IMotorService motorService)
        {
            _foorPlanService = foorPlanService;
            _userMotorRepository = userMotorRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _motorService = motorService;
            UserId = this._httpContextAccessor.HttpContext!.User.Claims.Where(x => x.Type.Contains("nameidentifier")).FirstOrDefault()!.Value;
            UserName = this._httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Contains("emailaddress")).FirstOrDefault()!.Value;
        }

        public async Task<bool> Add(ResponseMotorModel motorModel, RequestUserMotorModel userMotorModel)
        {
            var contractUserFoorPlans = new List<ResponseContractUserFoorPlanModel>();
            try
            {
                var motorModelContract = _mapper.Map<MotorModelContract>(motorModel);

                var flag = await _motorService.UpdateMotorFlag(motorModelContract);

                if (!flag)
                    return false;

                var countDay = userMotorModel.ContractUserFoorPlanModel!.First().FloorPlanCountDay;
                var foorPlan = _foorPlanService.GetByCountDay(countDay);
                var forCastEndDate = DateTime.Parse(userMotorModel.ContractUserFoorPlanModel!.First().ForecastEndDate);

                var userMotor = BuildContracts.BuildContract(userMotorModel, foorPlan, _mapper);
                userMotor.UserName = UserName;
                userMotor.UserId = UserId;

                await Task.Run(() => _userMotorRepository.Add(userMotor));

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void Delete(string id)
        {
            _userMotorRepository.Delete(id);
        }

        public async Task<IEnumerable<ResponseContractUserMotorModel>> Get(string? id, string? plate)
        {
            var userMotorModels = new List<ResponseContractUserMotorModel>();
            var usersMotors = await Task.Run(() => _userMotorRepository.Get(id, plate));
            if (usersMotors != null && usersMotors.Any())
            {
                foreach (var userMotor in usersMotors)
                {
                    var userMotorModel = _mapper.Map<ResponseContractUserMotorModel>(userMotor);

                    foreach (var item in userMotor.ContractUserFoorPlan!)
                    {
                        userMotorModel.ContractUserFoorPlanModel!.Add(new()
                        {
                            EndDate = item.EndDate,
                            FloorPlanCountDay = item.FloorPlanCountDay,
                            ForecastEndDate = item.ForecastEndDate,
                            Id = item.Id,
                            MotorPlate = item.MotorPlate,
                            PenaltyMissingDaysValue = item.PenaltyMissingDaysValue,
                            PenaltyOverDaysValue = item.PenaltyOverDaysValue,
                            StarDate = item.StarDate,
                            CountCurrentDays = item.CountCurrentDays
                        });
                        userMotorModels.Add(userMotorModel);
                    }
                }
                return userMotorModels;
            }
            return userMotorModels;
        }

        public async Task<ResponseCnhModel> UpdateCnh(RequestCnhUpdateModel cnhUserMotorImage)
        {
            var userId = UserId;
            var userMotor = await Task.Run(() => _userMotorRepository.Get(userId, null).FirstOrDefault());
            userMotor!.Cnh!.ImagenCnh = cnhUserMotorImage.ImagenCnh;
            _userMotorRepository.Update(userMotor);

            userMotor = await Task.Run(() => _userMotorRepository.Get(userId, null).FirstOrDefault());

            var result = _mapper.Map<ResponseCnhModel>(userMotor!.Cnh);
            return result;
        }

        public async Task<ModelControllerValidation> ValidInputsController(RequestUserMotorModel userMotorModel)
        {
            var modelValided = new ModelControllerValidation();
            modelValided.Message = string.Empty;
            modelValided.IsValid = false;

            var existUser = _userMotorRepository.GetByUserId(UserId);
            if (existUser != null)
            {
                modelValided.Message = $"User {existUser.UserName} is already registered";
                return modelValided;
            }

            var existCpfCnpj = _userMotorRepository.GetByCpfCnpj(userMotorModel.CpfCnpj);
            if (existCpfCnpj != null)
            {
                modelValided.Message = $"Cpf or Cnpj {existCpfCnpj.CpfCnpj} is already registered";
                return modelValided;
            }

            var existCnh = _userMotorRepository.GetCnh(userMotorModel.Cnh!.NumberCnh);
            if (existCnh != null)
            {
                modelValided.Message = $"Cnh {existCnh!.NumberCnh} is already registered";
                return modelValided;
            }

            DateTime birthDate;
            var isValidateDate = DateTime.TryParse(userMotorModel.BirthDate, out birthDate);
            if (!isValidateDate)
            {
                modelValided.Message = "BirthDate invalid";
                return modelValided;
            }

            string pattern = @"^[*a*,b]+$";
            var match = Regex.Match(string.Join(",", userMotorModel.Cnh!.CnhCategories!), pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                modelValided.Message = "You are not qualified to rent a motorcycle or Cnh category invalid";
                return modelValided;
            }

            DateTime forecastEndDate;
            isValidateDate = DateTime.TryParse(userMotorModel.ContractUserFoorPlanModel!.Select(x => x.ForecastEndDate).First(), out forecastEndDate);
            if (!isValidateDate)
            {
                modelValided.Message = "ForecastEndDate invalid";
                return modelValided;
            }



            if (forecastEndDate < DateTime.Now.AddDays(1))
            {
                modelValided.Message = "Expected delivery date should be as of tomorrow.";
                return modelValided;
            }

            var foorPlan = _foorPlanService.GetByCountDay(userMotorModel.ContractUserFoorPlanModel!.FirstOrDefault()!.FloorPlanCountDay);
            if (foorPlan == null)
            {
                modelValided.Message = "There are no plans for that number of days";
                return modelValided;
            }

            var motorsAvailable = await _motorService.GetMotorsAvailableToRental();

            var motorPlateRequest = userMotorModel.ContractUserFoorPlanModel!.First().MotorPlate;

            var motorAvailable = motorsAvailable.Where(x => x.Plate!.ToUpper() == motorPlateRequest.ToUpper());

            if (motorAvailable.Count() == 0)
            {
                modelValided.Message = $"Motorcycle {string.Join(',', motorPlateRequest)} is not available to rented";
                return modelValided;
            }
            modelValided.MotorAvailable = motorAvailable.FirstOrDefault()!;
            modelValided.IsValid = true;
            return modelValided;
        }
    }
}
