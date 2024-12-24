using Echoes.Data;
using Echoes.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Echoes.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CommentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Add(int PostId, string Content)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var comment = new Comment
            {
                Content = Content,
                PostId = PostId,
                UserId = userId.Value,
                Likes = 0
            };

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("GetPost", "Posts", new { id = PostId });
        }
    }
}
