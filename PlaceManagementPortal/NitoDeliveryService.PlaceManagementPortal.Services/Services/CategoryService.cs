using AutoMapper;
using NiteDeliveryService.Shared.DAL.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using NitoDeliveryService.PlaceManagementPortal.Services.Interfaces;
using NitoDeliveryService.Shared.Models.PlaceDTOs;
using System;
using System.Threading.Tasks;

namespace NitoDeliveryService.PlaceManagementPortal.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewCoategory(CategoryDTO category)
        {
            var categoryEntity = _mapper.Map<CategoryDTO, Category>(category);

            var result = await _categoryRepository.Create(categoryEntity);

            if (result != null)
            {
                throw new Exception("Error creating menu category");
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveCategory(int categoryId)
        {
            var result = await _categoryRepository.Delete(categoryId);

            if (!result)
            {
                throw new Exception("Error removing menu category");
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
