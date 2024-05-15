namespace vesta_api.Database.Models;

public partial class Client
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Address { get; set; } = null!;

    public int DocumentId { get; set; }
    
    public bool IsActive { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Document Document { get; set; } = null!;

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<Testing> Testings { get; set; } = new List<Testing>();

    public virtual ICollection<ResponsibleForClient>? ResponsiblesForClient { get; set; } =
        new List<ResponsibleForClient>();
}