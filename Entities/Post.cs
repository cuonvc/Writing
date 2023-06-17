using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Writing.Payloads.DTOs;

namespace Writing.Entities; 

[Table("Posts_tbl")]
public class Post : BaseEntity {
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    [MaybeNull]
    public string Description { get; set; }
    
    [MaybeNull]
    public string Thumbnail { get; set; }
    
    public int Vote { get; set; } = 0;
    
    public int View { get; set; } = 0;
    
    public bool Pined { get; set; } = false;

    public bool IsPending { get; set; } = false;

    public User User { get; set; }

    public List<Category> Categories { get; set; } = new List<Category>();
    
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
}