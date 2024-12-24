using Echoes.Data;
using Echoes.Models;
using Echoes.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Echoes.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AddUserViewModel viewModel)
        {
            if(viewModel.Password != viewModel.RePassword)
            {
                ViewBag.ErrorMessage = "Password does not match";
                return View();
            }

            var user = new User
            {
                UserName = viewModel.UserName,
                Password = viewModel.Password,
                ActiveStatus = 0
            };

            await dbContext.Users.AddAsync(user);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            var user = await dbContext.Users
        .FirstOrDefaultAsync(u => u.UserName == viewModel.UserName && u.Password == viewModel.Password);

            if (user is not null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.UserName);
                    
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View(viewModel);
        }


        [HttpGet] // READ (List)
        public async Task<IActionResult> Accounts()
        {
            var users = await dbContext.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
        }

        [HttpGet] // UPDATE (Read)
        public async Task<IActionResult> Edit(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            return View(user);  

        }

        [HttpPost] // UPDATE
        public async Task<IActionResult> Edit(User viewModel)
        {
            var user = await dbContext.Users.FindAsync(viewModel.UserId);

            if (user is not null)
            {
                user.UserName = viewModel.UserName;
                user.Password = viewModel.Password;

                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Accounts", "Users");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(User viewModel)
        {
            var student = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == viewModel.UserId);

            if (student is not null)
            {
                dbContext.Users.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Accounts", "Users");
        }
    }
}
