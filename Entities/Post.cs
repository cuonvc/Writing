using System.ComponentModel.DataAnnotations.Schema;

namespace Writing.Entities; 

[Table("Posts_table")]
public class Post : BaseEntity {
    
    public string Title { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Thumbnail { get; set; }
    public int Upvote { get; set; } = 0;
    public int Downvote { get; set; } = 0;
    public int View { get; set; }
    public bool pined { get; set; }
    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
}