namespace vesta_api.Database.Models.View.Requests;

public partial class CreateResponsibleForClientRequest
{
    public int ResponsibleId { get; set; }

    public int ClientId { get; set; }
}