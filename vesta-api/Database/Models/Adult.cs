namespace vesta_api.Database.Models;

public partial class Adult
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string IdentityDocument { get; set; } = null!;

    public string Type { get; set; } = null!;
}