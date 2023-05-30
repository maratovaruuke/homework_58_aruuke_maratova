using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using homework_59_aruuke_maratova.Models;
using homework_59_aruuke_maratova.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace homework_59_aruuke_maratova.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private InstagramContext _context;
        private readonly UserManager<User> _userManager;

        public PostsController(InstagramContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            ViewBag.UserId = _userManager.GetUserId(User);
            return View(new CreatePostViewModel { UserId = user.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePostViewModel model)
        {
            if (model != null)
            {
                Post post = new Post
                {
                    CreatedAt = DateTime.Now,
                    UserId = model.UserId,
                    Caption = model.Caption,
                    Img = GetImg(model)
                };
                _context.Posts.Add(post);
                _context.SaveChanges();
            }

            return RedirectToAction("Feed", "Instagram");
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            Post post = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments).FirstOrDefault(p => p.Id == int.Parse(id));

            foreach (var comment in post.Comments)
            {
                comment.User = _context.Users.Find(comment.UserId);
            }

            ViewBag.UserId = _userManager.GetUserId(User);
            ViewBag.time = PostsController.PostUploadDate(post.CreatedAt);
            return View(post);
        }

        public static string PostUploadDate(DateTime createdAt)
        {
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var ts = new TimeSpan(DateTime.Now.Ticks - createdAt.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * minute)
                return "a minute ago";

            if (delta < 45 * minute)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * minute)
                return "an hour ago";

            if (delta < 24 * hour)
                return ts.Hours + " hours ago";

            if (delta < 48 * hour)
                return "yesterday";

            if (delta < 30 * day)
                return ts.Days + " days ago";

            if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        private static byte[] GetImg(CreatePostViewModel model)
        {
            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(model.Img.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)model.Img.Length);
            }
            return imageData;
        }
    }
}