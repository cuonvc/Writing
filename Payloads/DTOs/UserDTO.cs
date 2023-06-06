using Writing.Entities;
using Writing.Enumerates;

namespace Writing.Payloads.DTOs; 

public class UserDTO : BaseDTO {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string About { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string AvatarPhoto { get; set; }
    public string CoverPhoto { get; set; }
}