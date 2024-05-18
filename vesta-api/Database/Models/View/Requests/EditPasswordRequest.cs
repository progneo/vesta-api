namespace vesta_api.Database.Models.View.Requests;

public class EditPasswordRequest
{
    public int UserId { get; set; }
    
    public string Password { get; set; } = string.Empty;
}