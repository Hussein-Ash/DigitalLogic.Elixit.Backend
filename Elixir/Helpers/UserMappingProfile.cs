using AutoMapper;
using Elixir.DATA.DTOs;
using Elixir.DATA.DTOs.CategoryDto;
using Elixir.DATA.DTOs.CommonQuestions;
using Elixir.DATA.DTOs.CreditCard;
using Elixir.DATA.DTOs.Follow;
using Elixir.DATA.DTOs.Like;
using Elixir.DATA.DTOs.Order;
using Elixir.DATA.DTOs.Product;
using Elixir.DATA.DTOs.ProductCategory;
using Elixir.DATA.DTOs.ProductComment;
using Elixir.DATA.DTOs.ProductLik;
using Elixir.DATA.DTOs.ReportProducts;
using Elixir.DATA.DTOs.Store;
using Elixir.DATA.DTOs.StoreAddres;
using Elixir.DATA.DTOs.StoreCategory;
using Elixir.DATA.DTOs.User;
using Elixir.DATA.DTOs.UserAddressDto;
using Elixir.DATA.DTOs.UserSave;
using Elixir.DATA.DTOs.UserStore;
using Elixir.Entities;
using OneSignalApi.Model;
using Polly.Caching;


namespace Elixir.Helpers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {


            CreateMap<AppUser, UserDto>();
            CreateMap<AppUser, UsersDto>()
            .ForMember(x => x.Role, y => y.MapFrom(t => t.Role.ToString()));
            

            
            CreateMap<RegisterForm, App>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AppUser, AppUser>();


            CreateMap<AppUser, UserDto>();
            CreateMap<RegisterForm, App>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(src => src.SubCategory));

            CreateMap<CategoryForm, Category>();
            CreateMap<CategoryUpdate, Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserAddress, UserAddressDto>();
            CreateMap<UserAddressForm, UserAddress>();
            CreateMap<UserAddressUpdate, UserAddress>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Store, StoreDto>();
            CreateMap<StoreForm, Store>();
            CreateMap<StoreUpdate, Store>();

            CreateMap<ReportProduct, ReportProductDto>();
            CreateMap<ReportProductForm, ReportProduct>();
            CreateMap<ReportProductUpdate, ReportProduct>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));



            CreateMap<ProductComment, ProductCommentDto>()
            .ForMember(x => x.UserName, y => y.MapFrom(t => t.User.UserName))
            .ForMember(x => x.UserImg, y => y.MapFrom(t => t.User.Imgs.Any()
                                    ? t.User.Imgs.LastOrDefault() : "default img"))
            .ForMember(x => x.ReplyComments, y => y.MapFrom(t => t.ReplyComments));


            CreateMap<ProductCommentForm, ProductComment>();
            CreateMap<ProductCommentUpdate, ProductComment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<UserSave, UserSaveDto>();
            CreateMap<UserSaveForm, UserSave>();


            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<ProductCategoryForm, ProductCategory>();
            CreateMap<ProductCategoryUpdate, ProductCategory>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CommonQuestion, CommonQuestionDto>();
            CreateMap<CommonQuestionForm, CommonQuestion>();
            CreateMap<CommonQuestionUpdate, CommonQuestion>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));



            CreateMap<StoreAddres, StoreAddresDto>();
            CreateMap<StoreAddresForm, StoreAddres>();
            CreateMap<StoreAddresUpdate, StoreAddres>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.StoreName, x => x.MapFrom(y => y.Store.Name));
            CreateMap<ProductForm, Product>();
            CreateMap<ProductUpdate, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Order, OrderDto>();

            CreateMap<OrderForm, Order>();
            CreateMap<OrderUpdate, Order>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));



            CreateMap<CreditCard, CreditCardDto>();
            CreateMap<CreditCardForm, CreditCard>();
            CreateMap<CreditCardUpdate, CreditCard>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<StoreCategory, StoreCategoryDto>();
            CreateMap<StoreCategoryForm, StoreCategory>();

            CreateMap<UserStore, UserStoreDto>();
            CreateMap<UserStoreForm, UserStore>();


            CreateMap<ProductInOrder, ProductInOrderDto>()
            .ForMember(dest => dest.StoreName, x => x.MapFrom(y => y.Product.Store.Name))
            .ForMember(dest => dest.ProductName, x => x.MapFrom(y => y.Product.Name));

            CreateMap<ProductInOrder, StoreOrdersDto>()
            .ForMember(dest => dest.StoreName, x => x.MapFrom(y => y.Product.Store.Name))
            .ForMember(dest => dest.ProductName, x => x.MapFrom(y => y.Product.Name))
            .ForMember(dest => dest.OrderState, x => x.MapFrom(y => y.Order.Status.ToString()))
            .ForMember(dest => dest.CustomerName, x => x.MapFrom(y => y.Order.User.UserName))
            .ForMember(dest => dest.TotalPrice, x => x.MapFrom(y => y.Order.TotalPrice));


            CreateMap<Follow, FollowDto>()
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.UserId.HasValue ? src.UserId : src.StoreId))
                .ForMember(dest => dest.Img,
                    opt => opt.MapFrom(src => src.User != null && src.User.Imgs != null && src.User.Imgs.Any()
                                      ? src.User.Imgs.LastOrDefault()
                                      : (src.Store != null && src.Store.Imgs != null && src.Store.Imgs.Any()
                                          ? src.Store.Imgs.LastOrDefault()
                                          : null)))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.User != null ? src.User.FullName : src.Store.Name))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User != null ? src.User.UserName : src.Store.NickName))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Follower.ToString()));


            CreateMap<Like, LikeDto>()
                .ForMember(dest => dest.LikedId,
                    opt => opt.MapFrom(src => src.ProductId ?? src.CommentId))
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Img,
                    opt => opt.MapFrom(src => src.User != null && src.User.Imgs.Any()
                                    ? src.User.Imgs.LastOrDefault() : "default img"))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type.ToString()));

        }
    }
}