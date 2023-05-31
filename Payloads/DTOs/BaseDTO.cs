namespace Writing.Payloads.DTOs; 

public class BaseDTO {
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public bool isActive { get; set; }
}