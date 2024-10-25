using GhostyNetwork.Core.Application.Dtos.Email;

namespace GhostyNetwork.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}