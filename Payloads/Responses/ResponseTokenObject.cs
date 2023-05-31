using Writing.Entities;

namespace Writing.Payloads.Responses; 

public class ResponseTokenObject {

    public string TokenType { get; set; } = "Bearer";
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpireDate { get; set; }
}