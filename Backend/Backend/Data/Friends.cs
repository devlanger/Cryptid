using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data
{
    public class FriendsPair
    {
        [Key]
        public int RelationId { get; set; }

        public Guid PlayerId { get; set; }

        public Guid FriendId { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
