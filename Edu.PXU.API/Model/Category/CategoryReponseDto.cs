namespace Edu.PXU.API.Model.Category
{
    public class CategoryReponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdate { get; set; }

    }
}
