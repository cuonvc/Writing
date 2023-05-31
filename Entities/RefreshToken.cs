using System.ComponentModel.DataAnnotations.Schema;

namespace Writing.Entities; 

[Table("RefreshTokens_tbl")]
public class RefreshToken : BaseEntity {
    
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public User User { get; set; }
}