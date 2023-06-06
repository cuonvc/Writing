using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Writing.Entities; 

[Table("UserPostVote_tbl")]
public class UserPostVote : BaseEntity {

    [MaybeNull]
    public User User { get; set; }
    
    [MaybeNull]
    public Post Post { get; set; }
    
    public string VoteType { get; set; }

}