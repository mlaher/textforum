using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace textforum.domain.models
{
    public class Post
    {
        public long PostId { get; set; }
        public long UserId { get; set; }
        public required string Content { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public List<PostLike> Likes { get; set; } = new List<PostLike>();
        public List<PostComment> Comments { get; set; } = new List<PostComment>();
        public List<PostTag> Tags { get; set; } = new List<PostTag>();
    }
}
