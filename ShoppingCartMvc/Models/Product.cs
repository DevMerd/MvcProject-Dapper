using System.ComponentModel.DataAnnotations;

namespace ShoppingCartMvc.Models
{
    public class Product
    {
        public decimal Price { get; set; }
        public int ProductId { get; set; }     
        [Required]
        public string Name { get; set; }
        [Range(1,100,ErrorMessage="Quantity must be between 1 and 100 only")]
        public int Quantity { get; set; }
    }
}
