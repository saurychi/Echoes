using Echoes.Data;
using Microsoft.AspNetCore.Mvc;

namespace Echoes.Controllers
{
    public class LikesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public LikesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddLike(int postId)
        {
            var post = await dbContext.Posts.FindAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            post.LikeCount++;
            dbContext.Posts.Update(post);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("GetPost", "Posts", new { id = postId });
        }
    }
}
