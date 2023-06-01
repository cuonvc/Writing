using System.Text.Json.Serialization;
using Writing.Entities;

namespace Writing.Payloads.DTOs; 

public class PostDTO : BaseDTO {
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public string Description { get; set; }
    
    public string Thumbnail { get; set; }
    
    public int VoteUp { get; set; } = 0;
    
    public int VoteDown { get; set; } = 0;
    
    public int View { get; set; } = 0;
    
    public bool Pined { get; set; } = false;
    
    public UserDTO User { get; set; }

    public List<CategoryDTO> Categories { get; set; }
    
}