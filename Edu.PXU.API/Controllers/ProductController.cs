using AutoMapper;
using AutoWrapper.Wrappers;
using Edu.PXU.API.App.Interface;
using Edu.PXU.API.Model;
using Edu.PXU.API.Model.Category;
using Edu.PXU.API.Model.Product;
using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Edu.PXU.API.Model.Images;
using static System.Net.WebRequestMethods;

namespace Edu.PXU.API.Controllers
{
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProductController(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Tạo mới sản phẩm
        /// Tạo mới hình ảnh sản phẩm
        /// </summary>
        /// <param name="inp"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/product/create")]
        public async Task<ApiResponse> Create([FromForm] ProductRequestDto inp)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(inp.IdCategory);
                if (category == null)
                {
                    return new ApiResponse("Không tìm thấy danh mục này", StatusCodes.Status404NotFound);
                }


                List<string> fileNames = UploadFile(inp.ImagePro);

                var product = _mapper.Map<Product>(inp);
                product.Category = category;

                List<Image> images = new List<Image>();
                if (inp.ImagePro != null)
                {
                    foreach (var item in fileNames)
                    {
                        var image = new Image
                        {
                            FileName = item,

                        };

                        images.Add(image);
                    }
                }

                List<ProductImage> productImages = new List<ProductImage>();
                foreach (var item in images)
                {
                    var productImage = new ProductImage
                    {
                        Image = item,
                        Product = product,

                    };
                    productImages.Add(productImage);
                }
                //  product.ProductImages = productImages;


                await _unitOfWork.ProductRepository.CreateAsync(product);
                await _unitOfWork.ImageRepository.CreateAsync(images);
                await _unitOfWork.ProductImageRepository.CreateAsync(productImages);
                _unitOfWork.SaveChanges();
                return new ApiResponse("Thêm mới sản phẩm thành công", inp, StatusCodes.Status200OK);
                //return new ApiResponse("Thêm mới sản phẩm không thành công", StatusCodes.Status500InternalServerError);

            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/product/update")]
        public async Task<ApiResponse> Update([FromBody] ProductUpdateDto inp, string id)
        {
            try
            {
                var isProduct = await _unitOfWork.ProductRepository.GetByIdAsync(id);

                if (isProduct == null)
                {
                    return new ApiResponse("Không tìm thấy sản phẩm trong hệ thống", StatusCodes.Status404NotFound);
                }

                Category isCategory = new Category();
                if (!string.IsNullOrEmpty( inp.IdCategory))
                {
                     isCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(inp.IdCategory);
                    if(isCategory == null)
                    {
                        return new ApiResponse("Không tìm thấy danh mục trong hệ thống", StatusCodes.Status404NotFound);

                    }
                }


                isProduct.Name = inp.Name;
                isProduct.Price = inp.Price;
                if (!string.IsNullOrEmpty(isCategory.Name))
                {
                    isProduct.Category = isCategory;
                }
              

                var resp = _unitOfWork.ProductRepository.Update(isProduct, isProduct.Id);
                await _unitOfWork.SaveChangesAsync();

                return new ApiResponse("cập nhật thông tin thành công", StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/product/update-images-product")]
        public async Task<ApiResponse> UpdateFileImage([FromForm] ImageUpdateDto inp, string idProduct)
        {
            try
            {


                var isProduct = await _unitOfWork.ProductRepository.GetByIdAsync(idProduct);
                if (isProduct == null)
                {
                    return new ApiResponse("Không tìm thấy sản phẩm trong hệ thống", StatusCodes.Status404NotFound);
                }
                
                for(int i = 0; i < inp.IdImage.Count(); i++)
                {
                    var isImage = await _unitOfWork.ImageRepository.GetByIdAsync(inp.IdImage[i]);
                    if (isImage == null)
                    {
                        return new ApiResponse($"Không tìm thấy hình ảnh {inp.IdImage[i]} trong hệ thống", StatusCodes.Status404NotFound);
                    }

                    ChangeFileImageInFolder(isImage.FileName, inp.FileImage[i]);
                }
                //foreach (var item in inp.IdImage)
                //{
                //    var isImage = await _unitOfWork.ImageRepository.GetByIdAsync(item);
                //    if (isImage == null)
                //    {
                //        return new ApiResponse($"Không tìm thấy hình ảnh {item} trong hệ thống", StatusCodes.Status404NotFound);
                //    }

                //    ChangeFileImageInFolder(isImage.FileName, item.FileImage);
                //}

                return new ApiResponse("cập nhật file iamge thành công", StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message, StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet]
        [Route("api/product/GetAll")]
        public async Task<ApiResponse> GetAll([FromQuery] string? idCategory, [FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            try
            {
               
                var products = _unitOfWork.ProductRepository.GetAll<Product>(
                    match: x => idCategory == null || x.IdCategory == idCategory,
                    pageSize: pageSize,
                    pageIndex: pageIndex,
                    out int total,
                    include: g => g.Include(x => x.ProductImages)
                                    .ThenInclude(x => x.Image)
                                    );

                var data = _mapper.Map<List<ProductReponseDto>>(products);

                return new ApiResponse("Danh sách sản phẩm", data, StatusCodes.Status200OK);



            }
            catch (Exception e)
            {
                return new ApiResponse("Lỗi hệ thống! Vui lòng thử lại sau", StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("api/product/delete")]
        public async Task<ApiResponse> Delete([Required] string idProduct)
        {
            try
            {
                var data =  await  _unitOfWork.ProductRepository.GetAllIncludeAsync(match: x => x.Id == idProduct,
                    include: x => x.Include(x => x.ProductImages)
                                    .ThenInclude(x => x.Image)
                                    );
                
                _unitOfWork.ProductRepository.RemoveAll((ICollection<Product>)data.ToList());
                await _unitOfWork.SaveChangesAsync();

                return new ApiResponse($"Xóa sản phẩm có Id {idProduct} thành công", StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return new ApiResponse("Lỗi hệ thống! Vui lòng thử lại sau", StatusCodes.Status500InternalServerError);
            }
        }

        private List<string> UploadFile(List<IFormFile> files)
        {

            List<string> fileNames = new List<string>();
            string wwwPath = this._hostingEnvironment.WebRootPath;
            string contentPath = this._hostingEnvironment.ContentRootPath;

            string path = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in files)
            {
                // Kiểm tra file đầu vào là file ảnh
                if (postedFile.ContentType.ToLower().StartsWith("image/"))
                {
                    // Kiểm tra kích thước file
                    if (postedFile.Length <= 5 * 1024 * 1024) // Giới hạn kích thước file 5MB
                    {
                        // Tạo tên file ngẫu nhiên
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
                        fileNames.Add(fileName);
                        try
                        {
                            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                            {
                                postedFile.CopyTo(stream);
                                uploadedFiles.Add(fileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Lưu file thất bại. Lỗi: {ex.Message}");
                        }
                    }
                    else
                    {
                        throw new Exception("Kích thước file vượt quá 5MB! Vui lòng chọn file khác");
                    }
                }
                else
                {
                    throw new Exception("Vui lòng chọn định dạng file ảnh");
                }
            }
            return fileNames;
        }

        private void ChangeFileImageInFolder(string fileName, IFormFile file)
        {
            List<string> fileNames = new List<string>();
            string wwwPath = this._hostingEnvironment.WebRootPath;
            string contentPath = this._hostingEnvironment.ContentRootPath;

            string path = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(path))
            {
               throw new Exception("Không tìm thấy thư mục lưu ảnh!");
            }

            // xóa file cũ ra hệ thống
            // Create or open the file for writing
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                // Copy the uploaded file to the file stream
                file.CopyTo(stream);
            }


        }
    }
}
