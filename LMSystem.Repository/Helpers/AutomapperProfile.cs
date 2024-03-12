using AutoMapper;
using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<AccountModel, Account>().ReverseMap();
            CreateMap<WishListModel, WishList>().ReverseMap();
            CreateMap<NotificationModel, Notification>().ReverseMap();
            CreateMap<OrderPaymentModel, Order>().ReverseMap();
        }
    }
}
