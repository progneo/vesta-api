namespace vesta_api.Database.Models;

public class NewUserViewModel
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
}