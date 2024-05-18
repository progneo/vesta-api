namespace vesta_api.Database.Models.View.Requests;

public class CreateEmployeeRequest
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;
}