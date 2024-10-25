namespace GhostyNetwork.Core.Application.Helpers
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string plainPassword, string hashedPassword);
    }
}