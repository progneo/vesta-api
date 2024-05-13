namespace vesta_api.Database.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Duration { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}