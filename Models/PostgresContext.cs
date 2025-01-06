using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ADPC_Project_1.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Checkup> Checkups { get; set; }

    public virtual DbSet<Medicalfile> Medicalfiles { get; set; }

    public virtual DbSet<Medicalrecord> Medicalrecords { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=cosmically-magnetic-pug.data-1.euc1.tembo.io;Port=5432;Database=postgres;Username=postgres;Password=Br4q3w8Pkd0wO5VC;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "pg_cron")
            .HasPostgresExtension("columnar")
            .HasPostgresExtension("parquet_s3_fdw")
            .HasPostgresExtension("pg_partman")
            .HasPostgresExtension("pg_stat_statements")
            .HasPostgresExtension("postgres_fdw")
            .HasPostgresExtension("tier", "pg_tier");

        modelBuilder.Entity<Checkup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("checkups_pkey");

            entity.ToTable("checkups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Checkupdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("checkupdate");
            entity.Property(e => e.Patientid).HasColumnName("patientid");
            entity.Property(e => e.Procedurecode)
                .HasMaxLength(10)
                .HasColumnName("procedurecode");

            entity.HasOne(d => d.Patient).WithMany(p => p.Checkups)
                .HasForeignKey(d => d.Patientid)
                .HasConstraintName("checkups_patientid_fkey");
        });

        modelBuilder.Entity<Medicalfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medicalfiles_pkey");

            entity.ToTable("medicalfiles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Checkupid).HasColumnName("checkupid");
            entity.Property(e => e.Filename)
                .HasMaxLength(255)
                .HasColumnName("filename");
            entity.Property(e => e.Filepath)
                .HasMaxLength(255)
                .HasColumnName("filepath");
            entity.Property(e => e.Uploaddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("uploaddate");

            entity.HasOne(d => d.Checkup).WithMany(p => p.Medicalfiles)
                .HasForeignKey(d => d.Checkupid)
                .HasConstraintName("medicalfiles_checkupid_fkey");
        });

        modelBuilder.Entity<Medicalrecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medicalrecords_pkey");

            entity.ToTable("medicalrecords");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Diseasename)
                .HasMaxLength(255)
                .HasColumnName("diseasename");
            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.Patientid).HasColumnName("patientid");
            entity.Property(e => e.Startdate).HasColumnName("startdate");

            entity.HasOne(d => d.Patient).WithMany(p => p.Medicalrecords)
                .HasForeignKey(d => d.Patientid)
                .HasConstraintName("medicalrecords_patientid_fkey");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("patients_pkey");

            entity.ToTable("patients");

            entity.HasIndex(e => e.Personalidentificationnumber, "patients_personalidentificationnumber_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Personalidentificationnumber)
                .HasMaxLength(50)
                .HasColumnName("personalidentificationnumber");
            entity.Property(e => e.Sex)
                .HasMaxLength(1)
                .HasColumnName("sex");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("prescriptions_pkey");

            entity.ToTable("prescriptions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Checkupid).HasColumnName("checkupid");
            entity.Property(e => e.Dosage)
                .HasMaxLength(100)
                .HasColumnName("dosage");
            entity.Property(e => e.Duration)
                .HasMaxLength(100)
                .HasColumnName("duration");
            entity.Property(e => e.Medicationname)
                .HasMaxLength(255)
                .HasColumnName("medicationname");

            entity.HasOne(d => d.Checkup).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.Checkupid)
                .HasConstraintName("prescriptions_checkupid_fkey");
        });
        modelBuilder.HasSequence("jobid_seq", "cron");
        modelBuilder.HasSequence("row_mask_seq", "columnar");
        modelBuilder.HasSequence("runid_seq", "cron");
        modelBuilder.HasSequence("storageid_seq", "columnar").HasMin(10000000000L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
