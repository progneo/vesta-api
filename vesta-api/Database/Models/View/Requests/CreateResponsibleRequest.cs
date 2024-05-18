namespace vesta_api.Database.Models.View.Requests;

public partial class CreateResponsibleRequest
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int DocumentId { get; set; }
}