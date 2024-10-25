using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Application.ViewModels.Comments;
using GhostyNetwork.Core.Application.ViewModels.Friendships;
using GhostyNetwork.Core.Application.ViewModels.Reply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GhostyNetworkApp.Controllers
{
    [Authorize]
    public class FriendshipController : Controller
    {
        private readonly IFriendshipService _friendshipService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;
        private readonly IUserService _userService;

        public FriendshipController(IFriendshipService friendshipService, ICommentService commentService,
            IReplyService replyService, IUserService userService)
        {
            _friendshipService = friendshipService;
            _commentService = commentService;
            _replyService = replyService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> FriendPosts()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var friendPosts = await _friendshipService.GetPostsFromFriendsAsync(userId);
            var friends = await _friendshipService.GetFriendsAsync(userId);

            var viewModel = new FriendViewModel
            {
                Friends = friends,
                FriendPosts = friendPosts
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchFriends(string searchTerm)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var allUsers = await _userService.SearchUsersAsync(searchTerm);

                if (allUsers == null || !allUsers.Any())
                {
                    TempData["ErrorMessage"] = "No se encontraron usuarios con ese nombre.";
                    return RedirectToAction(nameof(FriendPosts));
                }

                var confirmedFriends = await _friendshipService.GetFriendsAsync(userId);

                var friendsToAdd = allUsers
                    .Where(user => user.UserId != userId)
                    .ToList();

                var viewModel = new FriendViewModel
                {
                    Friends = confirmedFriends,
                    FriendPosts = await _friendshipService.GetPostsFromFriendsAsync(userId),
                    SearchResults = friendsToAdd,
                    UserName = searchTerm
                };

                return View("FriendPosts", viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al buscar amigos: " + ex.Message;
                return RedirectToAction(nameof(FriendPosts));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFriend(string friendUsername)
        {
            var currentUsername = User.Identity.Name;
            await _friendshipService.AddFriendByUsernameAsync(currentUsername, friendUsername);


            TempData["SuccessMessage"] = "Amigo agregado con éxito.";
            return RedirectToAction(nameof(FriendPosts));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFriend(int friendId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _friendshipService.RemoveFriendshipAsync(userId, friendId);

                TempData["SuccessMessage"] = "Amistad eliminada con éxito.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al eliminar la amistad: " + ex.Message;
            }

            return RedirectToAction(nameof(FriendPosts));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CreateCommentViewModel createCommentViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _commentService.CreateCommentAsync(createCommentViewModel, userId);
                TempData[$"SuccessMessage_{createCommentViewModel.PostId}"] = "Comentario realizado con éxito.";
                return RedirectToAction(nameof(FriendPosts));
            }

            TempData[$"ErrorMessage_{createCommentViewModel.PostId}"] = "No puedes enviar un comentario vacío.";
            return RedirectToAction(nameof(FriendPosts));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReply(CreateReplyViewModel createReplyViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _replyService.CreateReplyAsync(createReplyViewModel, userId);
                TempData[$"SuccessMessage_Reply_{createReplyViewModel.CommentId}"] = "Respuesta realizada con éxito.";
                return RedirectToAction(nameof(FriendPosts));
            }

            TempData[$"ErrorMessage_Reply_{createReplyViewModel.CommentId}"] =
                "No puedes enviar la respuesta del comentario vacía.";
            return RedirectToAction(nameof(FriendPosts));
        }
    }
}