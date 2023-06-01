using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Writing.Entities; 

[Table("Categories_table")]
[Index("Name", IsUnique = true)]
public class Category : BaseEntity {

    public string Name { get; set; }
    
    public List<Post> Posts { get; set; } = new List<Post>();
}