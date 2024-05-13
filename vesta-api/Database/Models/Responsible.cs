namespace vesta_api.Database.Models;

public partial class Responsible
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int DocumentId { get; set; }

    public virtual Document Document { get; set; } = null!;
}