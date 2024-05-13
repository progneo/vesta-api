namespace vesta_api.Database.Models;

public partial class Document
{
    public int Id { get; set; }

    public string Series { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Type { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Responsible> Responsibles { get; set; } = new List<Responsible>();
}