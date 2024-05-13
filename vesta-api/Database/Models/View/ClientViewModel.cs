namespace vesta_api.Database.Models.View;

public class ClientViewModel
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Address { get; set; } = null!;

    public string IdentityDocumentType { get; set; } = null!;

    public string IdentityDocumentSerial { get; set; } = null!;

    public string IdentityDocumentNumber { get; set; } = null!;
}