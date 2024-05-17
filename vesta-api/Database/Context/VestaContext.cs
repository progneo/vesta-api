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

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Responsible> Responsibles { get; set; }

    public virtual DbSet<ResponsibleForClient> ResponsibleForClients { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<TestAnswerOfClient> TestAnswerOfClients { get; set; }

    public virtual DbSet<TestQuestion> TestQuestions { get; set; }

    public virtual DbSet<TestQuestionAnswer> TestQuestionAnswers { get; set; }

    public virtual DbSet<TestQuestionCategory> TestQuestionCategories { get; set; }

    public virtual DbSet<Testing> Testings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("appointment_pk");

            entity.ToTable("appointment");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Datetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Client).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_client_id_fk");

            entity.HasOne(d => d.Employee).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_employee_id_fk");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_service_id_fk");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_pk");

            entity.ToTable("client");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasColumnType("character varying")
                .HasColumnName("address");
            entity.Property(e => e.BirthDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("birth_date");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.Patronymic)
                .HasColumnType("character varying")
                .HasColumnName("patronymic");
            entity.Property(e => e.Sex)
                .HasColumnType("character varying")
                .HasColumnName("sex");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnType("boolean")
                .HasColumnName("is_active");

            entity.HasOne(d => d.Document).WithMany(p => p.Clients)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("client_document_id_fk");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("document_pk");

            entity.ToTable("document");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Number)
                .HasColumnType("character varying")
                .HasColumnName("number");
            entity.Property(e => e.Series)
                .HasColumnType("character varying")
                .HasColumnName("series");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("table_name_pk");

            entity.ToTable("employee");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.Patronymic)
                .HasColumnType("character varying")
                .HasColumnName("patronymic");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("note_pk");

            entity.ToTable("note");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Text)
                .HasColumnType("character varying")
                .HasColumnName("text");

            entity.HasOne(d => d.Client).WithMany(p => p.Notes)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("note_client_id_fk");
        });

        modelBuilder.Entity<Responsible>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("adult_pk");

            entity.ToTable("responsible");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.Patronymic)
                .HasColumnType("character varying")
                .HasColumnName("patronymic");
            entity.Property(e => e.PhoneNumber)
                .HasColumnType("character varying")
                .HasColumnName("phone_number");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");

            entity.HasOne(d => d.Document).WithMany(p => p.Responsibles)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("responsible_document_id_fk");
        });

        modelBuilder.Entity<ResponsibleForClient>(entity =>
        {
            entity.ToTable("responsible_for_client");

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.ResponsibleId).HasColumnName("responsible_id");

            entity.HasKey(e => new { e.ClientId, e.ResponsibleId });

            entity
                .HasOne(e => e.Client)
                .WithMany(e => e.ResponsiblesForClient)
                .HasForeignKey(e => e.ClientId)
                .HasConstraintName("client_id");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("service_pk");

            entity.ToTable("service");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<TestAnswerOfClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("test_answer_of_client_pk");

            entity.ToTable("test_answer_of_client");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AnswerId).HasColumnName("answer_id");
            entity.Property(e => e.TestingId).HasColumnName("testing_id");

            entity.HasOne(d => d.Answer).WithMany(p => p.TestAnswerOfClients)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("test_answer_of_client_test_question_answer_id_fk");

            entity.HasOne(d => d.Testing).WithMany(p => p.TestAnswersOfClient)
                .HasForeignKey(d => d.TestingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("test_answer_of_client_testing_id_fk");
        });

        modelBuilder.Entity<TestQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("test_question_pk");

            entity.ToTable("test_question");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsMultipleChoice)
                .HasDefaultValue(false)
                .HasColumnName("is_multiple_choice");
            entity.Property(e => e.Text)
                .HasColumnType("character varying")
                .HasColumnName("text");

            entity.HasOne(d => d.Category).WithMany(p => p.TestQuestions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("test_question_test_question_category_id_fk");
        });

        modelBuilder.Entity<TestQuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("test_question_answer_pk");

            entity.ToTable("test_question_answer");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.Question).WithMany(p => p.TestQuestionAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("test_question_answer_test_question_id_fk");
        });

        modelBuilder.Entity<TestQuestionCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("test_question_category_pk");

            entity.ToTable("test_question_category");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Testing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("testing_pk");

            entity.ToTable("testing");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Datetime)
                .HasDefaultValue(DateTime.Now)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Testings)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("testing_client_id_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pk");

            entity.ToTable("user");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.PasswordKey).HasColumnName("password_key");
            entity.Property(e => e.Role)
                .HasColumnType("character varying")
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");

            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_employee_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}