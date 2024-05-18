namespace vesta_api.Database.Models.View.Requests;

public class EditUserRequest
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }
}