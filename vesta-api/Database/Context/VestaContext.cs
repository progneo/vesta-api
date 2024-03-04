﻿using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Models;

namespace vesta_api.Database.Context;

public partial class VestaContext : DbContext
{
    public VestaContext()
    {
    }

    public VestaContext(DbContextOptions<VestaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Father> Fathers { get; set; }

    public virtual DbSet<Mother> Mothers { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

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

            entity.HasOne(d => d.Client).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Appointment_clientId_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Appointment_employeeId_fkey");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Client_pkey");

            entity.ToTable("Client");

            entity.HasIndex(e => e.FatherId, "Client_fatherId_key").IsUnique();

            entity.HasIndex(e => e.MotherId, "Client_motherId_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.BirthDate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("birthDate");
            entity.Property(e => e.FatherId).HasColumnName("fatherId");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.IdentityDocument).HasColumnName("identityDocument");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.MotherId).HasColumnName("motherId");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");

            entity.HasOne(d => d.Father).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.FatherId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Client_fatherId_fkey");

            entity.HasOne(d => d.Mother).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.MotherId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Client_motherId_fkey");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Employee_pkey");

            entity.ToTable("Employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adress).HasColumnName("adress");
            entity.Property(e => e.BirhtDate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("birhtDate");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Phone).HasColumnName("phone");
        });

        modelBuilder.Entity<Father>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Father_pkey");

            entity.ToTable("Father");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.IdentityDocument).HasColumnName("identityDocument");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Phone).HasColumnName("phone");
        });

        modelBuilder.Entity<Mother>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Mother_pkey");

            entity.ToTable("Mother");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.IdentityDocument).HasColumnName("identityDocument");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Phone).HasColumnName("phone");
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

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("id");
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
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordHash");
            entity.Property(e => e.PasswordKey).HasColumnName("passwordKey");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Username).HasColumnName("username");

            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("User_employeeId_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("User_roleId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}