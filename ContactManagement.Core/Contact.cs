using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactManagement.Core;

public class Contact
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [DisplayName("First Name")]
    [Required, StringLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [DisplayName("Last Name")]
    [Required, StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    [DisplayName("Company Name")]
    public string? CompanyName { get; set; }
    [Required(ErrorMessage = "Mobile number is required")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number must be 10 digits")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Mobile number must contain only digits")]
    public string Mobile { get; set; } = string.Empty;
    [Required, StringLength(80, ErrorMessage = "Email cannot exceed 80 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$",
    ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
