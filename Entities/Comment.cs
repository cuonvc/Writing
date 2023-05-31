using System.ComponentModel.DataAnnotations.Schema;

namespace Writing.Entities; 

[Table("Comments_tbl")]
public class Comment : BaseEntity {

    public string content { get; set; }
    
}