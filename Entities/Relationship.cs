using System.ComponentModel.DataAnnotations.Schema;

namespace Writing.Entities; 

[Table("Relationships_tbl")]
public class Relationship : BaseEntity {
    public User? Follower { get; set; } //nullable
    public User? Following { get; set; }  //nullable
}