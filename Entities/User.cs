using System.ComponentModel.DataAnnotations.Schema;
using Writing.Enumerates;

namespace Writing.Entities; 

[Table("Users_tbl")]
public class User : BaseEntity {

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string About { get; set; }
    public Gender Gender { get; set; } = Gender.UNDEFINE;
    public Role Role { get; set; } = Role.USER_ROLE;
    public DateTime DateOfBirth { get; set; }
    public string AvatarPhoto { get; set; }
    public string CoverPhoto { get; set; }
    public List<Post> Posts { get; set; } = new List<Post>();
    public List<Comment> Comments = new List<Comment>();
}