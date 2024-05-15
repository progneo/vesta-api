namespace vesta_api.Database.Models.View;

public partial class ResponsibleViewModel
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int DocumentId { get; set; }
}