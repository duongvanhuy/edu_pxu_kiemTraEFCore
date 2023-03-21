namespace Edu.PXU.API.Model.Images
{
    public class ImageUpdateDto
    {
        public List<string> IdImage { get; set; }
        public List<IFormFile> FileImage { get; set; }
    }
}
