using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

public partial class LookdaysContext : DbContext
{
    public LookdaysContext()
    {
    }

    public LookdaysContext(DbContextOptions<LookdaysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivitiesModel> ActivitiesModels { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<ForumPost> ForumPosts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=lookdays;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivitiesModel>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("PK_Tags");

            entity.ToTable("ActivitiesModel");

            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ModelContent).HasMaxLength(50);
            entity.Property(e => e.ModelDate).HasColumnType("date");
            entity.Property(e => e.ModelName).HasMaxLength(50);
            entity.Property(e => e.ModelPrice).HasColumnType("money");

            entity.HasOne(d => d.Activity).WithMany(p => p.ActivitiesModels)
                .HasForeignKey(d => d.ActivityId)
                .HasConstraintName("FK_ActivitiesModel_Activities");
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.BookingStatesId).HasColumnName("BookingStatesID");
            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Activity).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Activities");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_User");
        });

        modelBuilder.Entity<ForumPost>(entity =>
        {
            entity.HasKey(e => e.PostId);

            entity.ToTable("ForumPost");

            entity.Property(e => e.PostId)
                .ValueGeneratedNever()
                .HasColumnName("PostID");
            entity.Property(e => e.PostContent)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.PostTime).HasColumnType("datetime");
            entity.Property(e => e.PostTitle).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ForumPosts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ForumPost_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(64);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(24);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
