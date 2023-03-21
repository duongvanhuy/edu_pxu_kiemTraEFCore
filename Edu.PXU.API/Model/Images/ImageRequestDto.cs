using Edu.PXU.EntityFECore.Entity;
using Microsoft.VisualBasic.FileIO;

namespace Edu.PXU.API.Model.Images
{
    public class ImageRequestDto
    {
        public string IdProduct { get; set; }
        
        public List<ImageUpdateDto> imageInfo { get; set; }
    }

}
