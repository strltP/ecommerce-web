using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.ViewModels
{
    public class CartViewModel
    {
        public int ProductId { get; set; }

    
        public int Count { get; set; }
    }
}
