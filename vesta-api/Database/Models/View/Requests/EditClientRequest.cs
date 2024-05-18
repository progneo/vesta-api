namespace vesta_api.Database.Models.View.Requests;

public class EditClientRequest
{
    public int Id { get; set; }
    
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Address { get; set; } = null!;

    public int DocumentId { get; set; }
    
    public bool IsActive { get; set; }
}