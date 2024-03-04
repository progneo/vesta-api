namespace vesta_api.Database.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public byte[] PasswordKey { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public int EmployeeId { get; set; }

    public int RoleId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}