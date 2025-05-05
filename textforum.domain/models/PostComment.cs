namespace textforum.domain.models
{
    public class PostComment
    {
        public long PostCommentId { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public required string Content { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    
}
