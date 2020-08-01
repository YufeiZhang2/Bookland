using System.Collections.Generic;

namespace BulkyBook.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> ListOfProductsInCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
        
        public int NewQuantity { get; set; }
    }
}