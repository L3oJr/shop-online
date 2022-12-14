using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Interfaces;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await this.shopOnlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId &&
                                                                     c.ProductId == productId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (this.shopOnlineDbContext.Products
                    .Where(product => product.Id == cartItemToAddDto.ProductId))
                    .Select(product => new CartItem
                    {
                        CartId = cartItemToAddDto.CartId,
                        ProductId = product.Id,
                        Qty = cartItemToAddDto.Qty,
                    }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await this.shopOnlineDbContext.CartItems.AddAsync(item);
                    await this.shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await this.shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                this.shopOnlineDbContext.CartItems.Remove(item);
                await this.shopOnlineDbContext.SaveChangesAsync();
            }

            return item;
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await this.shopOnlineDbContext.Carts
                .Join(this.shopOnlineDbContext.CartItems,
                    cart => cart.Id,
                    cartItem => cartItem.CartId,
                    (cart, cartItem) => new CartItem
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        Qty = cartItem.Qty,
                        CartId = cartItem.CartId
                    }
                ).Where(cartItem => cartItem.Id == id).SingleOrDefaultAsync();            
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await this.shopOnlineDbContext.Carts
                .Where(cart => cart.UserId == userId)
                .Join(this.shopOnlineDbContext.CartItems,
                    cart => cart.Id,
                    cartItem => cartItem.CartId,
                    (cart, cartItem) => new CartItem
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        Qty = cartItem.Qty,
                        CartId = cartItem.CartId,
                    }
                ).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await this.shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Qty= cartItemQtyUpdateDto.Qty;
                await this.shopOnlineDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}
