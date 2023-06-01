namespace Writing.Payloads.Requests; 

public class UserUpdateRequest {
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string About { get; set; }
    
    public string Gender { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
}