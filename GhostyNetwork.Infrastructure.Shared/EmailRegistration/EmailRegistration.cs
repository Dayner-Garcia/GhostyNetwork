using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Domain.Settings;
using GhostyNetwork.Infrastructure.Shared.Services;
using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GhostyNetwork.Infrastructure.Shared.EmailRegistration
{
    public static class EmailRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
