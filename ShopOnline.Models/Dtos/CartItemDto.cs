namespace ShopOnline.Models.Dtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImageURL { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public int Qty { get; set; }
    }
}
