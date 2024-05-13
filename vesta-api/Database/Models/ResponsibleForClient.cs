namespace vesta_api.Database.Models;

public partial class ResponsibleForClient
{
    public int ResponsibleId { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Responsible Responsible { get; set; } = null!;
}