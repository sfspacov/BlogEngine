using AutoMapper;
using BlogEngine.Core.Data.Entities;
using BlogEngine.Shared.DTOs.Blog;
using BlogEngine.Shared.DTOs.Comment;
using BlogEngine.Shared.DTOs.CustomerReview;
using BlogEngine.Shared.DTOs.Identity;

namespace BlogEngine.Api.Common.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            #region Post

            CreateMap<Post, PostDTO>()
                .ForMember(d => d.CommentDTOs, opt => opt.MapFrom(s => s.Comments))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.PostStatus.Description))
                ;

            CreateMap<PostCreationDTO, Post>()
                ;

            #endregion

            #region Comment

            CreateMap<CommentCreationDTO, Comment>().ReverseMap();

            CreateMap<Comment, CommentDTO>();


            CreateMap<CommentUpdateDTO, Comment>()
                .ForMember(mc => mc.ApplicationUserID, opt => opt.Ignore())
                .ForMember(mc => mc.PostID, opt => opt.Ignore());

            #endregion

            #region Identity

            CreateMap<UserLoginDTO, ApplicationUser>()
                .ForMember(au => au.Email, opt => opt.MapFrom(ul => ul.EmailAddress))
                .ForMember(au => au.UserName, opt => opt.MapFrom(ul => ul.EmailAddress));

            CreateMap<UserRegisterDTO, ApplicationUser>()
                .ForMember(au => au.Email, opt => opt.MapFrom(ur => ur.EmailAddress))
                .ForMember(au => au.UserName, opt => opt.MapFrom(ur => ur.EmailAddress));

            CreateMap<ApplicationUser, UserInfoDetailDTO>()
                .ForMember(ui => ui.EmailAddress, opt => opt.MapFrom(au => au.Email))
                .ForMember(ui => ui.ID, opt => opt.MapFrom(au => au.Id));

            CreateMap<ApplicationUser, UserProfileDTO>()
                .ForMember(up => up.PostDTOs, opt => opt.MapFrom(au => au.Posts))
                .ForMember(up => up.ID, opt => opt.MapFrom(au => au.Id))
                .ForMember(up => up.EmailAddress, opt => opt.MapFrom(au => au.Email));

            #endregion

            #region CustomerReview

            CreateMap<CustomerReviewCreationDTO, CustomerReview>();
            CreateMap<CustomerReview, CustomerReviewDTO>();

            #endregion
        }

    }
}