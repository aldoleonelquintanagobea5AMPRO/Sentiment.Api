using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sentiment.Api.Models;
using Sentiment.Api.Services;

namespace Sentiment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly SentimentService _sentiment;

        public CommentsController(AppDbContext db, SentimentService sentiment)
        {
            _db = db;
            _sentiment = sentiment;
        }

        public class CreateCommentRequest
        {
            public string Product_Id { get; set; } = "";
            public string User_Id { get; set; } = "";
            public string Comment_Text { get; set; } = "";
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCommentRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Comment_Text))
                return BadRequest("comment_text is required");

            var sentiment = _sentiment.Analyze(req.Comment_Text);

            var comment = new Comment
            {
                ProductId = req.Product_Id,
                UserId = req.User_Id,
                CommentText = req.Comment_Text,
                Sentiment = sentiment,
                CreatedAt = DateTime.UtcNow
            };

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? product_id, [FromQuery] string? sentiment)
        {
            var q = _db.Comments.AsQueryable();

            if (!string.IsNullOrEmpty(product_id))
                q = q.Where(c => c.ProductId == product_id);

            if (!string.IsNullOrEmpty(sentiment))
                q = q.Where(c => c.Sentiment == sentiment);

            var list = await q.OrderByDescending(c => c.CreatedAt).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var c = await _db.Comments.FindAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpGet("/api/sentiment-summary")]
        public async Task<IActionResult> SentimentSummary()
        {
            var total = await _db.Comments.CountAsync();
            var pos = await _db.Comments.CountAsync(c => c.Sentiment == "positivo");
            var neg = await _db.Comments.CountAsync(c => c.Sentiment == "negativo");
            var neu = await _db.Comments.CountAsync(c => c.Sentiment == "neutral");

            return Ok(new
            {
                total_comments = total,
                sentiment_counts = new
                {
                    positivo = pos,
                    negativo = neg,
                    neutral = neu
                }
            });
        }
    }
}

