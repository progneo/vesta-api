namespace vesta_api.Database.Models;

public partial class Note
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;
}