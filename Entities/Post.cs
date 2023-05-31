using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Writing.Entities; 

[Table("Posts_table")]
public class Post : BaseEntity {
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    [MaybeNull]
    public string Description { get; set; }
    
    [MaybeNull]
    public string Thumbnail { get; set; }
    
    public int VoteUp { get; set; } = 0;
    
    public int VoteDown { get; set; } = 0;
    
    public int View { get; set; } = 0;
    
    public bool pined { get; set; } = false;
    
    public List<Category> Categories { get; set; } = new List<Category>();
    
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
}