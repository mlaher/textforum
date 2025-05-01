using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace textforum.data.classes
{
    [Index(nameof(UserId))]
    [Index(nameof(PostId))]
    [Index(nameof(PostCommentId))]
    [Index(nameof(Timestamp))]
    public class PostComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PostCommentId { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public long PostId { get; set; }
        [Required]
        public string Content { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [Required]
        public DateTimeOffset Timestamp { get; set; }
    }

}
