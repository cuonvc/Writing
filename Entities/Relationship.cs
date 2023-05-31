using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Writing.Entities; 

[Table("Relationships_tbl")]
public class Relationship : BaseEntity {
    
    [MaybeNull]
    public User Follower { get; set; } //nullable
    
    [MaybeNull]
    public User Following { get; set; }  //nullable
    
}