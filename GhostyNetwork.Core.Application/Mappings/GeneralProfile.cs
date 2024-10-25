using AutoMapper;
using GhostyNetwork.Core.Application.ViewModels.Comments;
using GhostyNetwork.Core.Application.ViewModels.Friendships;
using GhostyNetwork.Core.Application.ViewModels.Posts;
using GhostyNetwork.Core.Application.ViewModels.Reply;
using GhostyNetwork.Core.Application.ViewModels.Users;
using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region "Login, Register and ResetPassword Mappings"

            CreateMap<User, RegisterViewModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<User, LoginViewModel>().ReverseMap();

            CreateMap<User, ResetPasswordViewModel>().ReverseMap();

            #endregion

            #region "UsersProfile"

            CreateMap<User, UserProfileViewModel>().ReverseMap();

            CreateMap<User, EditProfileViewModel>()
                .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore())
                .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore()).ReverseMap();

            #endregion

            #region "Posts"

            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePicture))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl));

            CreateMap<CreatePostViewModel, Post>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.ImagePath,
                    opt => opt.MapFrom(src => src.Image != null ? src.Image.FileName : null))
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl));

            CreateMap<EditPostViewModel, Post>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.Image != null ? src.Image.FileName : null))
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl));

            #endregion

            #region "Comments"

            CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePicture))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Created));

            CreateMap<CreateCommentViewModel, Comment>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow));

            #endregion

            #region "Reply"

            CreateMap<Reply, ReplyViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePicture));

            CreateMap<CreateReplyViewModel, Reply>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.CommentId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));

            #endregion

            #region "FriendShip"

            CreateMap<Friendship, FriendViewModel>()
                .ForMember(dest => dest.FriendId, opt => opt.MapFrom(src => src.FriendId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Friend.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Friend.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Friend.UserName))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.Friend.ProfilePicture));

            CreateMap<User, AvailableFriendViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            CreateMap<Friendship, AvailableFriendViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.FriendId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Friend.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Friend.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Friend.LastName))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.Friend.ProfilePicture));

            #endregion
        }
    }
}