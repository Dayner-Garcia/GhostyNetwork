using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GhostyNetworkApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (_userService.IsUserLoggedIn(HttpContext))
            {
                return RedirectToAction("Index", "Post");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userService.LoginAsync(loginViewModel.UserName, loginViewModel.Password);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        authProperties);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Post");
                }
                catch (UnauthorizedAccessException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }

            var success = await _userService.ResetPasswordAsync(resetPasswordViewModel.UserName);

            if (success)
            {
                TempData["SuccessMessage"] = "Se ha enviado un correo con la nueva contraseña.";
                return View(resetPasswordViewModel);
            }
            else
            {
                TempData["ErrorMessage"] = "El usuario no existe.";
                return View(resetPasswordViewModel);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.ProfilePicture == null)
            {
                ModelState.AddModelError("ProfilePicture", "No se ha subido ninguna foto de perfil.");
            }

            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            try
            {
                string profilePicturePath = await _userService.SavePhotoAsync(registerViewModel.ProfilePicture);
                var success = await _userService.RegisterAsync(registerViewModel, profilePicturePath);

                if (success)
                {
                    TempData["Message"] = "Te has Registrado correctamente. Revisa tu correo para activar tu cuenta.";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Activate(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var success = await _userService.ActivateUserAsync(token);
            if (success)
            {
                ViewBag.Message = "Cuenta activada exitosamente. Puedes iniciar sesión ahora.";
            }
            else
            {
                ViewBag.Message = "El token es inválido o ya ha sido utilizado.";
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = await _userService.GetCurrentUserProfileAsync(User);

            if (userProfile == null)
            {
                return RedirectToAction("Login");
            }

            var model = new EditProfileViewModel
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                Phone = userProfile.Phone,
                UserProfilePicture = userProfile.ProfilePicture
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editProfileViewModel)
        {
            var currentUserProfile = await _userService.GetCurrentUserProfileAsync(User);

            if (!ModelState.IsValid)
            {
                editProfileViewModel.UserProfilePicture = currentUserProfile.ProfilePicture;
                return View(editProfileViewModel);
            }

            string profilePicturePath = null;

            if (editProfileViewModel.ProfilePhoto != null)
            {
                try
                {
                    profilePicturePath = await _userService.SavePhotoAsync(editProfileViewModel.ProfilePhoto);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ProfilePhoto", "Error al subir la foto de perfil: " + ex.Message);
                    editProfileViewModel.UserProfilePicture = currentUserProfile.ProfilePicture;
                    return View(editProfileViewModel);
                }
            }

            else
            {
                profilePicturePath = currentUserProfile.ProfilePicture;
            }

            try
            {
                var success = await _userService.UpdateProfileAsync(editProfileViewModel, profilePicturePath, User);

                if (success)
                {
                    TempData["SuccessMessage"] =
                        "Perfil actualizado correctamente, por seguridad hemos enviado un mensaje a tu correo.";
                    return RedirectToAction("EditProfile");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "No se puede actualizar el perfil: " + ex.Message);
                editProfileViewModel.UserProfilePicture = currentUserProfile.ProfilePicture;
            }

            return View(editProfileViewModel);
        }
    }
}