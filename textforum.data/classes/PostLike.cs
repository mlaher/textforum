using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace textforum.data.classes
{
    [PrimaryKey(nameof(PostId), nameof(UserId))]
    [Index(nameof(UserId))]
    [Index(nameof(PostId))]
    [Index(nameof(Timestamp))]
    public class PostLike
    {
        [Required]
        public long PostId { get; set; }
        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [Required]
        public DateTimeOffset Timestamp { get; set; }
    }

}
