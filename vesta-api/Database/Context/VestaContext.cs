using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Models;

namespace vesta_api.Database.Context;

public partial class VestaContext : DbContext
{
    public VestaContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public VestaContext(DbContextOptions<VestaContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Adult> Adults { get; set; }

    public virtual DbSet<AdultOfClient> AdultsOfClient { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Appointment_pkey");

            entity.ToTable("Appointment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("clientId");
            entity.Property(e => e.DateTime)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("dateTime");
            entity.Property(e => e.EmployeeId).HasColumnName("employeeId");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");

            entity.HasOne(d => d.Client).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Appointment_clientId_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Appointment_employeeId_fkey");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Serviceid)
                .HasConstraintName("appointment_serviceid__fk");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Client_pkey");

            entity.ToTable("Client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.BirthDate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("birthDate");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.IdentityDocument).HasColumnName("identityDocument");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Employee_pkey");

            entity.ToTable("Employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
        });

        modelBuilder.Entity<Adult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Adult_pkey");

            entity.ToTable("Adult");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.IdentityDocument).HasColumnName("identityDocument");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Note_pkey");

            entity.ToTable("Note");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("clientId");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.Client).WithMany(p => p.Notes)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Note_clientId_fkey");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("service_pk");

            entity.ToTable("Service");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Test_pkey");

            entity.ToTable("Test");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("clientId");
            entity.Property(e => e.TestingDate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("testingDate");
            entity.Property(e => e.Answers).HasColumnName("answers");

            entity.HasOne(d => d.Client).WithMany(p => p.Tests)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Test_clientId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "User_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employeeId");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordHash");
            entity.Property(e => e.PasswordKey).HasColumnName("passwordKey");
            entity.Property(e => e.Username).HasColumnName("username");

            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("User_employeeId_fkey");
        });

        modelBuilder.Entity<AdultOfClient>(entity =>
        {
            entity.ToTable("AdultOfClient");

            entity.HasKey(e => new { e.ClientId, e.AdultId });

            entity
                .HasOne(e => e.Client)
                .WithMany(e => e.AdultsOfClient)
                .HasForeignKey(e => e.ClientId)
                .HasConstraintName("client_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<vesta_api.Database.Models.AdultOfClient> AdultOfClient { get; set; } = default!;
}