namespace textforum.domain.models
{
    public class PostLike
    {
        public long PostId { get; set; }
        public long UserId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    
}
