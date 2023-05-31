using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Writing.Enumerates;

namespace Writing.Entities; 

[Table("Users_tbl")]
[Index("Email", IsUnique = true)]
public class User : BaseEntity {

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public byte[] Salt { get; set; }
    
    [MaybeNull]
    public string About { get; set; }
    
    public string Gender { get; set; } = nameof(Enumerates.Gender.UNDEFINE);
    
    public string Role { get; set; } = nameof(Enumerates.Role.USER_ROLE);
    
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    
    [MaybeNull]
    public string AvatarPhoto { get; set; }
    
    [MaybeNull]
    public string CoverPhoto { get; set; }
    
    public List<Post> Posts { get; set; } = new List<Post>();
    
    public List<Comment> Comments = new List<Comment>();
    
}