using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GhostyNetwork.Infrastructure.Persistence
{
    public static class RepositoryRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            #region "Repositories"
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IReplyRepository, ReplyRepository>();
            services.AddTransient<IFriendshipRepository, FriendshipRepository>();
            #endregion
        }
    }
}
