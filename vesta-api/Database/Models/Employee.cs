namespace vesta_api.Database.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime BirhtDate { get; set; }

    public string Phone { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}