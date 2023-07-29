namespace WebApplication.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? Thumbnail { get; set; }
    }

    public class BasketItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}

//    public class SomeClass
//    {
//        public IEnumerable<Product>? Products { get; set; }

//        public IEnumerable<Product>? GetProducts()
//        {
//            return Products;
//        }

//        public void SomeMethod(IEnumerable<Product>? Products)
//        {
           
//            foreach (Product product in Products)
//            {
            
//            }
//        }
//    }

//}
