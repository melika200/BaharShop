using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Application.InterfaceRepository;
using BaharShop.Application.Services.Dto;
using BaharShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace BaharShop.Application.Services
{
    public class ShoppingCartService
    {

        private readonly IShoppingCartRepository _shoppingcartRepository;
        private readonly IProductRepository _productRepository;


        public ShoppingCartService(IShoppingCartRepository shoppingcartRepository, IProductRepository ProductRepository)
        {
            _shoppingcartRepository = shoppingcartRepository;
            _productRepository = ProductRepository;
        }
        public async Task<bool> AddToBasket(int productId, int qty, long userId)
        {
            // دریافت محصول
            var product = await _productRepository.GetById(productId);
            if (product == null || product.Stock <= 0)
            {
                return false; // محصول موجود نیست یا موجودی صفر است
            }

            // بررسی اینکه تعداد درخواستی بیشتر از موجودی نباشد
            if (qty > product.Stock)
            {
                return false; // تعداد درخواستی بیشتر از موجودی است
            }

            // کاهش موجودی محصول بلافاصله
            product.Stock -= qty;
            await _productRepository.Update(product); // به‌روزرسانی موجودی

            // دریافت سبد خرید فعال (Created)
            var shoppingCart = await _shoppingcartRepository.GetAll(a => a.UserId == userId && a.Status == Domain.Enum.Status.Created)
                .FirstOrDefaultAsync();

            // اگر سبد وجود ندارد، ایجاد سبد جدید
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    UserId = userId,
                    Status = Domain.Enum.Status.Created,
                    Address = "",
                    Mobile = "",
                    Payed = DateTime.Now,
                };
                await _shoppingcartRepository.Add(shoppingCart);
            }

            // بررسی اینکه آیا محصول قبلا در سبد وجود دارد یا خیر
            var existingItem = await _shoppingcartRepository.GetAllBasketItems(a => a.ShoppingCartId == shoppingCart.Id && a.ProductId == productId)
                .FirstOrDefaultAsync();

            if (existingItem != null)
            {
                // اگر محصول وجود داشت، تعداد را افزایش دهید و قیمت را به روز کنید
                existingItem.Quantity += qty;
                existingItem.Price = existingItem.Quantity * product.Price;
                await _shoppingcartRepository.UpdateBasketItem(existingItem);
            }
            else
            {
                // اگر محصول وجود نداشت، آیتم جدید اضافه کنید
                var shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = shoppingCart.Id,
                    Quantity = qty,
                    ProductId = product.Id,
                    Price = product.Price * qty,
                };
                await _shoppingcartRepository.AddBasketItem(shoppingCartItem);
            }

            return true;
        }


        public async Task<ShoppingCartItem> UpdateBasketItem(int itemId, int quantity)
        {
            var basketItem = await _shoppingcartRepository
     .GetAllBasketItems(a => a.Id == itemId)
     .Include(a => a.Product)
     .FirstOrDefaultAsync();
            if (basketItem == null || basketItem.Product == null)
            {
                return null;
            }

            basketItem.Quantity = quantity;
            basketItem.Price = basketItem.Product.Price * quantity;

            await _shoppingcartRepository.UpdateBasketItem(basketItem);
            return basketItem;
        }
        //لیست آیتم‌های سبد خرید کاربر را برمی‌گرداند.
        public async Task<List<ShoppingCartItem>> GetUserBasket(long userId)
        {
            var baskets = await _shoppingcartRepository.GetAllBasketItems(a => a.ShoppingCart.UserId == userId
             && a.ShoppingCart.Status == Domain.Enum.Status.Created)
                .Include(a => a.ShoppingCart)
                .Include(a => a.Product).AsNoTracking().ToListAsync(); //sorat mebara bala vli bayad vaghti estafada basha ka chezi ghara nesy update basha
                                                                       // اینجا محصول همراه آیتم‌ها بارگذاری می‌شود
            return baskets;
        }



        public async Task<bool> RemoveItemBasket(int Id)
        {
            var basketItem = await _shoppingcartRepository.GetAllBasketItems(a => a.Id == Id)
                .Include(a => a.Product)
                .FirstOrDefaultAsync();

            if (basketItem == null)
                return false;

            // افزایش موجودی محصول
            var product = basketItem.Product;
            product.Stock += basketItem.Quantity;
            await _productRepository.Update(product);

            // حذف آیتم از سبد خرید
            // حذف آیتم از سبد خرید
            await _shoppingcartRepository.DeleteBasketItem(basketItem);
            return true;
        }


        public async Task<bool> Pay(string mobile, string address, long userId)
        {
            var basket = await _shoppingcartRepository.GetAll(a => a.UserId == userId && a.Status == Domain.Enum.Status.Created)
                .FirstOrDefaultAsync();

            if (basket == null)
                return false;

            basket.Mobile = mobile;
            basket.Address = address;
            basket.Payed = DateTime.Now;
            basket.Status = Domain.Enum.Status.Final;

            await _shoppingcartRepository.Update(basket);

            return true;
        }





        public async Task<List<ShoppingCart>> GetUserOrders(long userId)
        {
            var baskets = await _shoppingcartRepository.GetAll(a => a.UserId == userId)  //Remove Status Filter
                .Include(a => a.Items)
                .ThenInclude(a => a.Product).AsNoTracking().ToListAsync();
            return baskets;
        }
        //Get All Orders For Admin
        public async Task<List<AdminOrderDto>> GetAllOrders()
        {
            var baskets = await _shoppingcartRepository.GetAll(a => a.Status != Domain.Enum.Status.Created)
                .Include(a => a.User)
                .Include(a => a.Items)
                .ThenInclude(a => a.Product)
                .Select(s => new AdminOrderDto()
                {
                    Address = s.Address,
                    Id = s.Id,
                    Mobile = s.Mobile,
                    Status = s.Status,
                    Payed = s.Payed,
                    UserId = s.UserId,
                    UserName = s.User.Username,
                    Items = s.Items.Select(c => c.Product.Name).ToList()
                })
                .AsNoTracking().ToListAsync();
            return baskets;
        }

        public async Task<AdminOrderDto> GetOrderWithId(long id)
        {
            var baskets = await _shoppingcartRepository.GetAll(a => a.Id == id)
                .Include(a => a.User)
                .Include(a => a.Items)
                .ThenInclude(a => a.Product)
                .Select(s => new AdminOrderDto()
                {
                    Address = s.Address,
                    Id = s.Id,
                    Mobile = s.Mobile,
                    Status = s.Status,
                    Payed = s.Payed,

                    UserId = s.UserId,
                    UserName = s.User.Username,
                    Items = s.Items.Select(c => c.Product.Name).ToList()
                })
                .AsNoTracking().FirstOrDefaultAsync();
            return baskets;
        }



        public async Task<bool> SetStatus(long Id, bool State)
        {
            var basket = await _shoppingcartRepository.GetAll(a => a.Id == Id).FirstOrDefaultAsync();
            if (State)
            {
                basket.Status = Domain.Enum.Status.Accepted;
            }
            else
            {
                basket.Status = Domain.Enum.Status.Rejected;

            }
            await _shoppingcartRepository.Update(basket);

            return true;
        }
    }
}
