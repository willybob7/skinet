using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        // public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        [MaxLength(100)]
        public string PictureUrl { get; set; }
        public virtual ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public virtual ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
    }
}