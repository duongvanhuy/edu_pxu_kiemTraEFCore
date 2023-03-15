using Edu.PXU.API.App.Interface;
using Edu.PXU.API.Model.Category;
using Edu.PXU.API.Model.Product;
using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Edu.PXU.API.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        [Route("api/product/create")]
        public ProductRequestDto Create(ProductRequestDto inp)
        {
            try
            {
                var product = new Product
                {
                    Name = inp.Name,
                    Code = inp.Code,
                    Price = inp.Price,
                    DateUpdate = inp.DateUpdate ?? DateTime.Now,
                    IdCategory = inp.IdCategory
                };
                var data = _unitOfWork.ProductRepository.Create(product);
                _unitOfWork.SaveChanges();
                return inp;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPut]
        [Route("api/product/update")]
        public ProductRequestDto Update(ProductRequestDto inp, int IdProduct)
        {
            try
            {
                var product = new Product
                {
                    Id = IdProduct,
                    Name = inp.Name,
                    Code = inp.Code,
                    Price = inp.Price,
                    DateUpdate = inp.DateUpdate ?? DateTime.Now,
                    IdCategory = inp.IdCategory
                };
                var data = _unitOfWork.ProductRepository.Update(product, IdProduct);
                return inp;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("api/product/Get")]
        public ProductReponseDto GetById(int IdProduct)
        {
            try
            {
                var categories = _unitOfWork.ProductRepository.Get(IdProduct);
                var product = new ProductReponseDto
                {
                    Id = categories.Id,
                    Name = categories.Name,
                    Code = categories.Code,
                    Price = categories.Price,
                    DateUpdate = categories.DateUpdate,
                    IdCategory = categories.IdCategory
                };

                return product;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("api/product/GetAll")]
        public ProductReponseDto GetAll()
        {
            try
            {
                var products = _unitOfWork.ProductRepository.GetAll();
               

                return new ProductReponseDto();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
