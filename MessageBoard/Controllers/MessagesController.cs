using MessageBoard.DataAccess;
using MessageBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MessageBoard.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MessageBoardContext _context;

        public MessagesController(MessageBoardContext context)
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
            var messages = _context.Messages
                .OrderBy(m => m.ExpirationDate)
                .ToList()
                .Where(m => m.IsActive()); // LINQ Where(), not EF Where()

            return View(messages);
        }

        public IActionResult AllMessages()
        {
            if (GetCurrentUser() is null || GetCurrentUser().Role != Role.Admin)
            {
                return NotFound();
            }
            var allMessages = new Dictionary<string, List<Message>>()
            {
                { "active" , new List<Message>() },
                { "expired", new List<Message>() }
            };

            foreach (var message in _context.Messages)
            {
                if (message.IsActive())
                {
                    allMessages["active"].Add(message);
                }
                else
                {
                    allMessages["expired"].Add(message);
                }
            }

            return View(allMessages);
        }

        public IActionResult New()
        {
            var currentUser = GetCurrentUser();

            if (currentUser is null)
            {
                return RedirectToAction("Login", "Users");
            }

            return View(currentUser);

        }

        [HttpPost]
        public IActionResult Create(int userId, string content, int expiresIn)
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null && currentUser.Id == userId)
            {
                _context.Messages.Add(
                    new Message() { 
                        Author = currentUser, 
                        Content = content, 
                        ExpirationDate = DateTime.UtcNow.AddDays(expiresIn) });

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private User? GetCurrentUser()
        {
            return _context.Users.Find(Convert.ToInt32(Request.Cookies["currentUser"]));
        }
    }
}
