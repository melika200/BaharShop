using BaharShop.Domain;

namespace BaharSite.ViewModel
{
    public class BasketViewModel
    {
        public List<ShoppingCartItem> Items { get; set; }
        public PaymentInfoViewModel PaymentInfo { get; set; }
    }
}
