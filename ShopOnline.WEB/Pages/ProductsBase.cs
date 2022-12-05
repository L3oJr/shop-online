using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services;
using ShopOnline.Web.Services.Interfaces;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }
        public string ErrorMsg { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await ClearLocalStorage();

                Products = await ManageProductsLocalStorageService.GetCollection();

                var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                var totalQty = shoppingCartItems.Sum(item => item.Qty);

                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
            }
            catch (Exception ex)
            {

                ErrorMsg = ex.Message;
            }
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return Products.GroupBy(p => p.CategoryId).OrderBy(productByGroup => productByGroup.Key);
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos)
        {
            return groupedProductDtos.FirstOrDefault(pg => pg.CategoryId == groupedProductDtos.Key).CategoryName;
        }

        private async Task ClearLocalStorage()
        {
            await ManageCartItemsLocalStorageService.RemoveCollection();
            await ManageProductsLocalStorageService.RemoveCollection();
        }
    }
}
