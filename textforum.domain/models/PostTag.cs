namespace textforum.domain.models
{
    public class PostTag
    {
        public string Tag { get; set; }
        public long PostId { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    
}
