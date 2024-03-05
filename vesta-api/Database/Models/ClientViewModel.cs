namespace vesta_api.Database.Models;

public partial class ClientViewModel
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Address { get; set; } = null!;

    public string IdentityDocument { get; set; } = null!;
}