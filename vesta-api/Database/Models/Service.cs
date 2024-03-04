using vesta_api.Database.Models;

namespace vesta_api;

public partial class Service
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Duration { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}