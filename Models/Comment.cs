using System;
using System.Collections.Generic;

namespace Sentiment.Api.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string CommentText { get; set; } = null!;

    public string Sentiment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
