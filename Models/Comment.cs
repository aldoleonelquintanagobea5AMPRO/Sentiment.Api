using System;
using System.Collections.Generic;

namespace Sentiment.Api.Models;

public partial class Comment
{
    public int Id { get; set; }

    public string ProductId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string CommentText { get; set; } = null!;

    public string Sentiment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
