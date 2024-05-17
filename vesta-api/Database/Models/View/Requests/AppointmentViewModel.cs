namespace vesta_api.Database.Models.View.Requests;

public partial class AppointmentViewModel
{
    public DateTime Datetime { get; set; }

    public int ClientId { get; set; }

    public int EmployeeId { get; set; }

    public int ServiceId { get; set; }
}