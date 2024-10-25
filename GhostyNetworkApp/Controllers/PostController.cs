using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Application.ViewModels.Comments;
using GhostyNetwork.Core.Application.ViewModels.Posts;
using GhostyNetwork.Core.Application.ViewModels.Reply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GhostyNetworkApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;

        public PostController(IPostService postService, ICommentService commentService, IReplyService replyService)
        {
            _postService = postService;
            _commentService = commentService;
            _replyService = replyService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //var posts = await _postService.GetRecentPostsAsync();
            var posts = await _postService.GetRecentPostsAsync(userId);
            return View(posts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel createPostViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var postImagePath = await _postService.SavePostImageAsync(createPostViewModel.Image);

                await _postService.CreatePostAsync(createPostViewModel, userId, postImagePath);

                return RedirectToAction("Index");
            }

            return View(createPostViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (post.UserId != userId)
            {
                return Unauthorized();
            }

            var editPostViewModel = new EditPostViewModel
            {
                Id = post.Id,
                UserId = post.UserId,
                Content = post.Content,
                VideoUrl = post.VideoUrl,
                Image = null
            };

            return View(editPostViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPostViewModel editPostViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editPostViewModel);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (editPostViewModel.UserId != userId)
            {
                return Unauthorized();
            }

            await _postService.UpdatePost(editPostViewModel, userId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (post.UserId != userId)
            {
                return Unauthorized();
            }

            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (post.UserId != userId)
            {
                return Unauthorized();
            }

            await _postService.DeletePost(id, userId);
            return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            }

            TempData[$"ErrorMessage_{createCommentViewModel.PostId}"] = "No puedes enviar un comentario vacío.";
            return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            }

            TempData[$"ErrorMessage_Reply_{createReplyViewModel.CommentId}"] =
                "No puedes enviar la respuesta del comentario vacía.";
            return RedirectToAction("Index");
        }
    }
}