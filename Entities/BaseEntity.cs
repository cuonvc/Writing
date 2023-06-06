using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Writing.Entities; 

public class BaseEntity {
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    
    [MaybeNull]
    public string CreatedBy { get; set; }
    
    [MaybeNull]
    public string ModifiedBy { get; set; }
    
    public bool IsActive { get; set; } = true;
}