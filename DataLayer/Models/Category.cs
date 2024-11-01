using System.ComponentModel.DataAnnotations;

namespace Menu_QRCode
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
        public ICollection<MenuItem> MenuItems { get; set; }
    }
}
