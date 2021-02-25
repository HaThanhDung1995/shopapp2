using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApplication.Application.ViewModels.Common;
using ShopApplication.Application.ViewModels.Product;
using ShopApplication.Data.Enums;
using ShopApplication.Ultilities.Extensions;

namespace ShopApplication.Models
{
    public class CheckoutViewModel : BillViewModel
    {
        public List<ShoppingCartViewModel> Carts { get; set; }
        public List<EnumModel> PaymentMethods
        {
            get
            {
                return ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                    .Select(c => new EnumModel
                    {
                        Value = (int)c,
                        Name = c.GetDescription()
                    }).ToList();
            }
        }
    }
}
