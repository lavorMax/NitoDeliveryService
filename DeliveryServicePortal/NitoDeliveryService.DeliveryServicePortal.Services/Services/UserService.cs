using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Models.DTOs;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateUser(UserDTO userDto)
        {
            var userEntity = _mapper.Map<UserDTO, User>(userDto);

            var result = await _userRepository.Create(userEntity);
            if (result != null)
            {
                throw new Exception("Error creating user");
            }

            await _unitOfWork.SaveAsync();

            return result.Id;
        }

        public async Task<UserDTO> GetUser(int userId)
        {
            var userEntity = await _userRepository.Read(userId);

            var result = _mapper.Map<User, UserDTO>(userEntity);

            return result;
        }

        public async Task UpdateUser(UserDTO userDto)
        {
            var userEntity = _mapper.Map<UserDTO, User>(userDto);

            var result = await _userRepository.Update(userEntity);
            if (!result)
            {
                throw new Exception("Error updating user");
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
