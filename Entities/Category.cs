using System.ComponentModel.DataAnnotations.Schema;

namespace Writing.Entities; 

[Table("Categories_table")]
public class Category : BaseEntity {

    public string Name { get; set; }
    
    public List<Post> Posts { get; set; } = new List<Post>();
}