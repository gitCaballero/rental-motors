using AutoMapper;
using RentalMotor.Api.Complement.Enums;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Services.Interfaces;

namespace RentalMotor.Api.Services.Implements
{
    public class UserMotorService: IUserMotorService
    {
        public readonly IUserMotorRepository _userMotorRepository;
        private readonly IMapper _mapper;
        public UserMotorService(IUserMotorRepository userMotorRepository, IMapper mapper ) 
        {
            _userMotorRepository = userMotorRepository;
            _mapper = mapper;
        }

        public void Add(UserMotorModel userMotorModel)
        {
            var listCategories = new List<string>();
            foreach (var category in userMotorModel.Cnh!.CnhCategories)
            {
                CnhCategory cnhCategory = (CnhCategory)category;
                listCategories.Add(cnhCategory.ToString());
            }
            var userMotor= _mapper.Map<UserMotor>(userMotorModel);
            userMotor.Cnh!.CnhCategories = listCategories;

            _userMotorRepository.AddUser(userMotor);
        }

        public void Delete(string id)
        {
            _userMotorRepository.DeleteUser(id);
        }

        public UserMotorModel GetById(string id)
        {
            var userMotor = _userMotorRepository.GetUserById(id);
            
            var userMotorModel = _mapper.Map<UserMotorModel>(userMotor);

            return userMotorModel;
        }

        public IEnumerable<UserMotorModel> Get()
        {
             var userMotorModels = new List<UserMotorModel>();
            var usersMotors = _userMotorRepository.GetUsers();
            if (usersMotors != null && usersMotors.Any())
            {
                foreach (var userMotor in usersMotors)
                {
                    var userMotorModel = _mapper.Map<UserMotorModel>(userMotor);
                    userMotorModels.Add(userMotorModel);
                }
                return userMotorModels;
            }
            return userMotorModels;
        }

        public void Update(UserMotorModel userMotorModel)
        {
            var userMotor = _mapper.Map<UserMotor>(userMotorModel);

            _userMotorRepository.UpdateUser(userMotor);
        }
    }
}
