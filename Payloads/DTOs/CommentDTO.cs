using Writing.Entities;

namespace Writing.Payloads.DTOs
{
    public class CommentDTO
    {
        public int CommentCount { get; set; }
        public List<Comment> ListComment { get; set; }
    }
}
