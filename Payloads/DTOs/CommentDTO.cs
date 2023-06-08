using Writing.Entities;

namespace Writing.Payloads.DTOs
{
    public class CommentDTO : BaseDTO {

        public string Content { get; set; }
        
        public UserDTO User { get; set; }
        
    }
}
