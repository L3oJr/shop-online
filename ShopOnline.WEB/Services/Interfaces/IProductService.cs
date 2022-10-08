using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetItems();
    }
}
