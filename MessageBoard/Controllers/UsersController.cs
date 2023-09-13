using MessageBoard.DataAccess;
using MessageBoard.Models;
using MessageBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MessageBoard.Controllers
{
    public class UsersController : Controller
    {
        private readonly MessageBoardContext _context;

        public UsersController(MessageBoardContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentUser = GetCurrentUser();

            if (currentUser != null)
            {
                ViewData["currentUserName"] = currentUser.Name;
                ViewData["CurrentUserRole"] = currentUser.Role;
            }
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            var currentUser = GetCurrentUser();
            if ( currentUser is null || currentUser.Role != Role.Admin )
            {
                return NotFound();
            }

            var users = _context.Users;

            return View(users);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, string password)
        {
            user.PasswordDigest = HashService.Digest(password);
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSession(string email, string password)
        {
            var users = _context.Users.ToList();
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user.PasswordDigest == HashService.Digest(password))
            {
                Response.Cookies.Append("currentUser", user.Id.ToString());
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("currentUser");

            return RedirectToAction("Index", "Home");
        }

        private User? GetCurrentUser()
        {
            return _context.Users.Find(Convert.ToInt32(Request.Cookies["currentUser"]));
        }
    }
}
