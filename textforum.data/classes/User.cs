using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace textforum.data.classes
{
    [Index(nameof(UserId))]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsModerator { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
        public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
        public ICollection<PostTag> Tags { get; set; } = new List<PostTag>();
    }

}
