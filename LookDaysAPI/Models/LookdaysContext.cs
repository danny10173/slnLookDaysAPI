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

    public virtual DbSet<ActionJoint> ActionJoints { get; set; }

    public virtual DbSet<ActionType> ActionTypes { get; set; }

    public virtual DbSet<ActivitiesAlbum> ActivitiesAlbums { get; set; }

    public virtual DbSet<ActivitiesModel> ActivitiesModels { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingState> BookingStates { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ClassJoint> ClassJoints { get; set; }

    public virtual DbSet<ClassName> ClassNames { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CreditCardInfo> CreditCardInfos { get; set; }

    public virtual DbSet<ForumPost> ForumPosts { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<ModelTag> ModelTags { get; set; }

    public virtual DbSet<ModelTagJoint> ModelTagJoints { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=lookdays;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActionJoint>(entity =>
        {
            entity.Property(e => e.ActionJointId).ValueGeneratedNever();

            entity.HasOne(d => d.ActionType).WithMany(p => p.ActionJoints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActionJoint_ActionType");

            entity.HasOne(d => d.Activity).WithMany(p => p.ActionJoints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActionJoint_Activities");

            entity.HasOne(d => d.User).WithMany(p => p.ActionJoints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActionJoint_User");
        });

        modelBuilder.Entity<ActionType>(entity =>
        {
            entity.Property(e => e.ActionTypeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<ActivitiesAlbum>(entity =>
        {
            entity.HasOne(d => d.Activity).WithMany(p => p.ActivitiesAlbums)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivitiesAlbum_Activities");
        });

        modelBuilder.Entity<ActivitiesModel>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("PK_Tags");

            entity.HasOne(d => d.Activity).WithMany(p => p.ActivitiesModels).HasConstraintName("FK_ActivitiesModel_Activities");
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasOne(d => d.City).WithMany(p => p.Activities).HasConstraintName("FK_Activities_City");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Activities).HasConstraintName("FK_Activities_Hotels");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasOne(d => d.Activity).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Activities");

            entity.HasOne(d => d.BookingStates).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_BookingStates");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_User");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK_Destinations");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Country");
        });

        modelBuilder.Entity<ClassJoint>(entity =>
        {
            entity.Property(e => e.ClassJointId).ValueGeneratedNever();

            entity.HasOne(d => d.Activity).WithMany(p => p.ClassJoints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassJoint_Activities");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassJoints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassJoint_ClassName");
        });

        modelBuilder.Entity<ClassName>(entity =>
        {
            entity.Property(e => e.ClassId).ValueGeneratedNever();
        });

        modelBuilder.Entity<CreditCardInfo>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.CreditCardInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditCardInfo_User");
        });

        modelBuilder.Entity<ForumPost>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.ForumPosts).HasConstraintName("FK_ForumPost_User");
        });

        modelBuilder.Entity<ModelTagJoint>(entity =>
        {
            entity.HasOne(d => d.Model).WithMany(p => p.ModelTagJoints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ModelTagJoint_ActivitiesModel");

            entity.HasOne(d => d.ModelTag).WithMany(p => p.ModelTagJoints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ModelTagJoint_ModelTags");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Bookings1");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasOne(d => d.Activity).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Activities");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
