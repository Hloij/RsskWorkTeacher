using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RsskWorkTeacher.ModelsDB;

public partial class RsskContext : DbContext
{
    public RsskContext()
    {
    }

    public RsskContext(DbContextOptions<RsskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cik> Ciks { get; set; }

    public virtual DbSet<Dostup> Dostups { get; set; }

    public virtual DbSet<Ktp> Ktps { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Rp> Rps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.24.4,49171; Database=rssk; User=dima1; Password=; Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cik>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cik1).HasColumnName("Cik");
        });

        modelBuilder.Entity<Dostup>(entity =>
        {
            entity.ToTable("Dostup");

            entity.Property(e => e.Id).HasMaxLength(16);
            entity.Property(e => e.Lvl).HasColumnName("lvl");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Ktp>(entity =>
        {
            entity.ToTable("Ktp");

            entity.Property(e => e.Id)
                .HasMaxLength(16)
                .HasColumnName("id");
            entity.Property(e => e.Ck).HasMaxLength(50);
            entity.Property(e => e.Ktp1).HasColumnName("Ktp");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Plan).HasMaxLength(50);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Ktp)
                .HasForeignKey<Ktp>(d => d.Id)
                .HasConstraintName("FK_Ktp_RP");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.YchebPlan);

            entity.Property(e => e.YchebPlan).HasMaxLength(255);
        });

        modelBuilder.Entity<Rp>(entity =>
        {
            entity.ToTable("RP");

            entity.Property(e => e.Id)
                .HasMaxLength(16)
                .HasColumnName("id");
            entity.Property(e => e.Ck).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Plan).HasMaxLength(50);
            entity.Property(e => e.Rp1).HasColumnName("Rp");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasMaxLength(16);
            entity.Property(e => e.IdDostup)
                .HasMaxLength(16)
                .HasColumnName("Id_Dostup");
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);

            entity.HasOne(d => d.IdDostupNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdDostup)
                .HasConstraintName("FK_User_Dostup");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
