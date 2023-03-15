using Edu.PXU.API.App.Interface;
using Edu.PXU.API.Model.Category;
using Microsoft.AspNetCore.Mvc;

namespace Edu.PXU.API.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("api/category/getAll")]
        public List<CategoryReponseDto> GetAll()
        {
            try
            {
                var categories = _unitOfWork.CategoryRepository.GetAll().Select( x => new CategoryReponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DateCreated = x.DateCreated,
                    DateUpdate = x.DateUpdate
                }).ToList();
                return categories;

            }
            catch(Exception e)
            {
                return new List<CategoryReponseDto>();
            }
        }
    }
}
