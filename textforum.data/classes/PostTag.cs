using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace textforum.data.classes
{
    [PrimaryKey(nameof(Tag), nameof(PostId))]
    [Index(nameof(UserId))]
    [Index(nameof(PostId))]
    [Index(nameof(Tag))]
    [Index(nameof(Timestamp))]
    public class PostTag
    {
        [Required]
        public string Tag { get; set; }

        [Required]
        public long PostId { get; set; }

        [Required]
        public long UserId { get; set; }
        
        public string Description { get; set; }

        public Post Post { get; set; }

        public User User { get; set; }
        [Required]
        public DateTimeOffset Timestamp { get; set; }
    }

}
