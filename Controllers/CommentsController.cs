using Microsoft.AspNetCore.Mvc;

namespace Sentiment.Api.Controllers
{
    public class CommentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
