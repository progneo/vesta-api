namespace vesta_api.Database.Models.View.Requests;

public class CreateDocumentRequest
{
    public string Series { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Type { get; set; } = null!;
}