using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Application.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GhostyNetwork.Core.Application.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static void AddAplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IReplyService, ReplyService>();
            services.AddTransient<IFriendshipService, FriendshipService>();
            #endregion
        }
    }
}
