using AutoMapper;
using AutoWrapper.Wrappers;
using Edu.PXU.API.App.Interface;
using Edu.PXU.API.Model.Category;
using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edu.PXU.API.Controllers
{
    [Authorize]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/category/getAll")]
        public async Task<ApiResponse> GetAll()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                var data = _mapper.Map<List<CategoryReponseDto>>(categories);
                return new ApiResponse("Get all category success", data, StatusCodes.Status200OK);

            }
            catch(Exception e)
            {
                return new ApiResponse("Get all category fail",  StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/category/create")]
        public async Task<ApiResponse> Create([FromBody] CategoryRequestDto inp)
        {
            try
            {
                var category = _mapper.Map<Category>(inp);
                
                await _unitOfWork.CategoryRepository.CreateAsync(category);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse("Create category success", inp, StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return new ApiResponse("Create category fail", StatusCodes.Status500InternalServerError);
            }
        }

    }
}
