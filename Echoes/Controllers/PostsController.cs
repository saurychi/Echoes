using Echoes.Data;
using Echoes.Models;
using Echoes.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Echoes.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PostsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Users");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPostViewModel viewModel)
        {
            var post = new Post
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                LikeCount = 0,
                UserId = viewModel.UserId
            };

            await dbContext.Posts.AddAsync(post);

            await dbContext.SaveChangesAsync();
            return RedirectToAction("Posts", "Posts");
        }

        [HttpGet]
        public async Task<IActionResult> Posts()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var posts = await dbContext.Posts
                .Where(p => p.UserId != userId)
                .ToListAsync();

            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> MyPosts()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var posts = await dbContext.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var comments = dbContext.Comments
                .Where(c => c.PostId == id)
                .Select(c => new CommentViewModel
                {
                    CommentContent = c.Content,
                    PostId = c.PostId,
                    CommenterId = c.UserId,
                    CommenterName = dbContext.Users
                        .Where(u => u.UserId == c.UserId)
                        .Select(u => u.UserName)
                        .FirstOrDefault()
                })
                .ToList();


            var author = dbContext.Users
                .Where(u => u.UserId == post.UserId)
                .Select(u => u.UserName)
                .FirstOrDefault();

            var authorId = post.UserId;

            var viewModel = new GetPostViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                LikeCount = post.LikeCount,
                Author = author,
                UserId = authorId,
                Comments = comments
            };  

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }
            var post = await dbContext.Posts
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Post viewModel)
        {
            var post = await dbContext.Posts.FindAsync(viewModel.PostId);

            if (post is not null)
            {
                post.Title = viewModel.Title;
                post.Content = viewModel.Content;

                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("GetPost", "Posts", new { id = viewModel.PostId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Post viewModel)
        {
            var student = await dbContext.Posts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PostId == viewModel.PostId);

            if (student is not null)
            {
                dbContext.Posts.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Posts", "Posts");
        }

    }
    }
