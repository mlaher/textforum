using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace textforum.data.classes
{
    [Index(nameof(UserId))]
    [Index(nameof(PostId))]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PostId { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Content { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
        public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
        public ICollection<PostTag> Tags { get; set; } = new List<PostTag>();
        [Required]
        public DateTimeOffset Timestamp { get; set; }
    }

}
