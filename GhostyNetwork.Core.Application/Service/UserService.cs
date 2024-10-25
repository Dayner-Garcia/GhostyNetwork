using AutoMapper;
using GhostyNetwork.Core.Application.Dtos.Email;
using GhostyNetwork.Core.Application.Helpers;
using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Application.ViewModels.Friendships;
using GhostyNetwork.Core.Application.ViewModels.Users;
using GhostyNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GhostyNetwork.Core.Application.Service
{
    public class UserService : GenericService<RegisterViewModel, UserProfileViewModel, User>, IUserService
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;


        public UserService(IGenericRepository<User> repository, IEmailService emailService, IMapper mapper,
            IUserRepository userRepository, IPasswordHasher passwordHasher) : base(repository, mapper)
        {
            _repository = repository;
            _emailService = emailService;
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserProfileViewModel> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("El usuario no existe. Por favor verifica tu nombre de usuario.");
            }

            if (user == null || !_passwordHasher.VerifyPassword(password, user.Password))
            {
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "El usuario no esta activo. Verifica tu correo para activar tu cuenta.");
            }

            return _mapper.Map<UserProfileViewModel>(user);
        }

        public bool IsUserLoggedIn(HttpContext httpContext)
        {
            var userIdClaim = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel registerViewModel, string profilePicturePath)
        {
            if (await UserExistsAsync(registerViewModel.Username))
            {
                throw new Exception("El nombre de usuario ya existe.");
            }

            if (await EmailExistsAsync(registerViewModel.Email))
            {
                throw new Exception("El correo electrónico ya está en uso.");
            }

            if (await PhoneNumberExistsAsync(registerViewModel.Phone))
            {
                throw new Exception("El número de teléfono ya está registrado.");
            }

            var user = _mapper.Map<User>(registerViewModel);

            user.ProfilePicture = profilePicturePath;

            user.Password = _passwordHasher.HashPassword(registerViewModel.Password);
            user.IsActive = false;
            user.ActivationToken = Guid.NewGuid().ToString();

            await _repository.AddAsync(user);

            var activationLink = $"http://localhost:5184/User/Activate?token={user.ActivationToken}";

            var emailRequest = new EmailRequest
            {
                To = user.Email,
                Subject = "Activación de cuenta",
                Body = $@"
            <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background: white; padding: 20px; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333;'>¡Hola {user.FirstName}!</h2>
                        <p style='color: #555;'>Para activar tu cuenta con el nombre de usuario: <strong>{user.UserName}</strong>, haz clic en el siguiente enlace:</p>
                        <a href='{activationLink}' style='background-color: #007bff; color: white; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>Activar Cuenta</a>
                        <p style='color: #555;'>Si no solicitaste esta activación, simplemente ignora este mensaje.</p>
                        <footer style='margin-top: 20px; border-top: 1px solid #eaeaea; padding-top: 10px;'>
                            <p style='color: #777;'>Gracias,</p>
                            <p style='color: #777;'>El equipo de GhostyNetworkApp</p>
                        </footer>
                    </div>
                </body>
            </html>"
            };
            await _emailService.SendAsync(emailRequest);

            return true;
        }

        public async Task<bool> ActivateUserAsync(string activationToken)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users
                .Where(u => u.ActivationToken == activationToken && !u.IsActive)
                .FirstOrDefault();

            if (user == null) return false;

            user.IsActive = true;

            await _repository.UpdateAsync(user, user.Id);

            await _emailService.SendAsync(new EmailRequest
            {
                To = user.Email,
                Subject = "Cuenta Activada",
                Body = $@"
            <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background: white; padding: 20px; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333;'>¡Hola {user.FirstName}!</h2>
                        <p style='color: #555;'>Tu cuenta ha sido activada con éxito.</p>
                        <p style='color: #555;'>Ya puedes iniciar sesión con tus credenciales.</p>
                        <footer style='margin-top: 20px; border-top: 1px solid #eaeaea; padding-top: 10px;'>
                            <p style='color: #777;'>Gracias por unirte a GhostyNetworkApp,</p>
                            <p style='color: #777;'>El equipo de Ghosty Interprinces</p>
                        </footer>
                    </div>
                </body>
            </html>"
            });
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null) return false;

            var newPassword = Guid.NewGuid().ToString().Substring(0, 8);
            user.Password = _passwordHasher.HashPassword(newPassword);

            await _repository.UpdateAsync(user, user.Id);

            await _emailService.SendAsync(new EmailRequest
            {
                To = user.Email,
                Subject = "Restablecimiento de Contraseña",
                Body = $@"
            <html>
                <body>
                    <h2>Hola {user.FirstName},</h2>
                    <p>Tu contraseña ha sido restablecida exitosamente.</p>
                    <p><strong>Tu nueva contraseña es:</strong> <span style='font-size: 1.2em; font-weight: bold; color: #007BFF;'>{newPassword}</span></p>
                    <p>Por favor, asegúrate de cambiarla después de iniciar sesión.</p>
                    <p>Si tienes problemas, no dudes en ponerte en contacto con Ghosty.</p>
                    <br />
                    <p>¡Gracias por ser parte de GhostyNetworkApp!</p>
                    <p>El equipo de Ghosty Interprinces</p>
                </body>
            </html>"
            });

            return true;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _userRepository.UserExistsAsync(username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return await _userRepository.PhoneNumberExistsAsync(phoneNumber);
        }

        public async Task<List<AvailableFriendViewModel>> SearchUsersAsync(string searchTerm)
        {
            var users = await _userRepository.SearchUsersAsync(searchTerm);
            return _mapper.Map<List<AvailableFriendViewModel>>(users);
        }

        public async Task<string> SavePhotoAsync(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return null;
            }

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/perfiles");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return $"/images/perfiles/{fileName}";
        }

        public async Task<UserProfileViewModel> GetCurrentUserProfileAsync(ClaimsPrincipal userClaims)
        {
            var userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return null;
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null) return null;

            return new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                ProfilePicture = user.ProfilePicture
            };
        }

        public async Task<bool> IsPhoneNumberTakenAsync(string phoneNumber, int userId)
        {
            var existingUserWithPhone = await _userRepository.GetByPhoneAsync(phoneNumber);
            return existingUserWithPhone != null && existingUserWithPhone.Id != userId;
        }

        public async Task<bool> UpdateProfileAsync(EditProfileViewModel editProfileViewModel, string profilePicturePath,
            ClaimsPrincipal userClaims)
        {
            var userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("Usuario no encontrado.");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("El usuario no fue encontrado.");
            }

            if (await EmailExistsAsync(editProfileViewModel.Email) && user.Email != editProfileViewModel.Email)
            {
                throw new Exception("El correo electrónico ya esta en uso por otro usuario.");
            }

            if (await IsPhoneNumberTakenAsync(editProfileViewModel.Phone, user.Id))
            {
                throw new Exception("El numero de telefono ya esta en uso por otro usuario.");
            }

            user.FirstName = editProfileViewModel.FirstName;
            user.LastName = editProfileViewModel.LastName;
            user.Phone = editProfileViewModel.Phone;

            if (user.Email != editProfileViewModel.Email)
            {
                user.Email = editProfileViewModel.Email;
            }

            if (!string.IsNullOrEmpty(editProfileViewModel.Password))
            {
                user.Password = _passwordHasher.HashPassword(editProfileViewModel.Password);
            }

            if (!string.IsNullOrEmpty(profilePicturePath))
            {
                user.ProfilePicture = profilePicturePath;
            }

            await _repository.UpdateAsync(user, user.Id);

            await _emailService.SendAsync(new EmailRequest
            {
                To = user.Email,
                Subject = "Perfil Actualizado",
                Body = $@"
            <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background: white; padding: 20px; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333;'>¡Hola {user.FirstName}!</h2>
                        <p style='color: #555;'>Tu perfil ha sido actualizado con éxito.</p>
                        <p style='color: #555;'>Si realizaste estos cambios, no necesitas hacer nada más. Si no fuiste tú, por favor contacta con Ghosty.</p>
                        <footer style='margin-top: 20px; border-top: 1px solid #eaeaea; padding-top: 10px;'>
                            <p style='color: #777;'>Gracias por ser parte de GhostyNetworkApp,</p>
                            <p style='color: #777;'>El equipo de GhostyNetworkApp</p>
                        </footer>
                    </div>
                </body>
            </html>"
            });
            return true;
        }
    }
}