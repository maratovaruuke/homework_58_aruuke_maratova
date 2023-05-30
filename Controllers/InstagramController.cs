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
    public class InstagramController : Controller
    {
        private InstagramContext _context;
        private readonly UserManager<User> _userManager;

        public InstagramController(InstagramContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Feed()
        {
            IQueryable<Post> posts = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt);
            
            ViewBag.Users = _userManager.Users;
            ViewBag.UserId = _userManager.GetUserId(User);
            return View(await posts.AsNoTracking().ToListAsync());
        }
        
        public async Task<IActionResult> Profile(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            user.Posts = _context.Posts.Where(u => u.UserId == user.Id).ToList();
            user.Followers = _context.Follows.Where(f => f.FolloweeId == user.Id).ToList();
            user.Following = _context.Follows.Where(f => f.FollowerId == user.Id).ToList();

            ViewBag.UserId = _userManager.GetUserId(User);
            return View(user);
        }
        
        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            EditProfileViewModel model = new EditProfileViewModel
                {User = await _userManager.FindByIdAsync(id)};
            return View(model);
        }
           
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel u)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(u.User.Id);

                user.Name = u.User.Name;
                user.UserName = u.User.UserName;
                user.Bio = u.User.Bio;
                user.Email = u.User.Email;
                user.PhoneNumber = u.User.PhoneNumber;
                user.Gender = u.User.Gender;

                if (u.NewAvatar != null)
                    user.Avatar = GetAvatar(u);

                await _userManager.UpdateAsync(user);
                return RedirectToAction("Profile", "Instagram", new { u.User.Id });
            }
            return View(u);
        }
        
        public async Task<IActionResult> Like(int id)
        {
            if (id != 0)
            {
                Post post = await _context.Posts.FindAsync(id);
                Like newLike = new Like
                {
                    CreatedAt = DateTime.Now,
                    UserId = _userManager.GetUserId(User),
                    PostId = post.Id
                };
                await _context.Likes.AddAsync(newLike);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Feed");
        }

        public async Task<IActionResult> RemoveLike(int id)
        {
            if (id != 0)
            {
                Post post = await _context.Posts.FindAsync(id);
                Like like = await _context.Likes.FirstOrDefaultAsync(l =>
                    l.PostId == post.Id && l.UserId == _userManager.GetUserId(User));
                _context.Entry(like).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Feed");
        }

        public async Task<IActionResult> Follow(string followeeId)
        {
            User followee = await _userManager.FindByIdAsync(followeeId);
            User follower = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            Follow newFollow = new Follow
            {
                FollowerId = follower.Id,
                FolloweeId = followee.Id
            };

            _context.Follows.Add(newFollow);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile", "Instagram", new {followee.Id});
        }

        public async Task<IActionResult> Unfollow(string followeeId)
        {
            User followee = await _userManager.FindByIdAsync(followeeId);
            User follower = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            Follow follow = await _context.Follows.FirstOrDefaultAsync(f => f.FolloweeId == followee.Id && f.FollowerId == follower.Id);
            
            _context.Entry(follow).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile", "Instagram", new { followee.Id });
        }

        public async Task<IActionResult> AddComment(int id, string text, string action)
        {
            Post post = await _context.Posts.FindAsync(id);

            Comment comment = new Comment
            {
                Text = text,
                CreatedAt = DateTime.Now,
                UserId = _userManager.GetUserId(User),
                PostId = post.Id
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            
            if (action == "Feed")
                return RedirectToAction("Feed");
            else
                return RedirectToAction("Details", "Posts", new { post.Id });
        }

        private static byte[] GetAvatar(EditProfileViewModel model)
        {
            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(model.NewAvatar.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)model.NewAvatar.Length);
            }
            return imageData;
        }
    }
}