using System.Diagnostics.CodeAnalysis;

namespace Writing.Payloads.Requests; 

public class ChangePasswordRequest {
    public string oldPassword { get; set; }
    public string newPassword { get; set; }
    
}