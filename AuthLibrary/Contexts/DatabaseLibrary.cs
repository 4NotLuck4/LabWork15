using System;
using System.Collections.Generic;
using AuthLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthLibrary.Contexts;

public partial class DatabaseLibrary : DbContext
{
    public DatabaseLibrary()
    {
    }

    public DatabaseLibrary(DbContextOptions<DatabaseLibrary> options)
        : base(options)
    {
    }

    public virtual DbSet<CinemaPrivilege> CinemaPrivileges { get; set; }

    public virtual DbSet<CinemaRolePrivilege> CinemaRolePrivileges { get; set; }

    public virtual DbSet<CinemaUser> CinemaUsers { get; set; }

    public virtual DbSet<CinemaUserRole> CinemaUserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=mssql;Initial Catalog=ispp3102;User ID=ispp3102;Password=3102;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CinemaPrivilege>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CinemaPr__3214EC07109B460D");

            entity.ToTable("CinemaPrivilege");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CinemaRolePrivilege>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PrivilegeId }).HasName("PK__CinemaRo__51C4B9FFD5F173A7");

            entity.ToTable("CinemaRolePrivilege");

            entity.HasOne(d => d.Privilege).WithMany(p => p.CinemaRolePrivileges)
                .HasForeignKey(d => d.PrivilegeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CinemaRol__Privi__2B0A656D");

            entity.HasOne(d => d.Role).WithMany(p => p.CinemaRolePrivileges)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CinemaRolePrivilege_CinemaUser");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.CinemaRolePrivileges)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CinemaRol__RoleI__2A164134");
        });

        modelBuilder.Entity<CinemaUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CinemaUs__3214EC071561484E");

            entity.ToTable("CinemaUser");

            entity.HasIndex(e => e.Login, "UQ__CinemaUs__5E55825BBDD5E55F").IsUnique();

            entity.Property(e => e.FailedLoginAttempts).HasDefaultValue(0);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UnlockDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<CinemaUserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CinemaUs__3214EC074546BBF9");

            entity.ToTable("CinemaUserRole");

            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
