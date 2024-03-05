using System.ComponentModel.DataAnnotations.Schema;

namespace vesta_api.Database.Models;

public partial class AdultOfClient
{
    [Column("adultId")] public int AdultId { get; set; }
    public Adult? Adult { get; set; }

    [Column("clientId")] public int ClientId { get; set; }
    public Client? Client { get; set; }
}