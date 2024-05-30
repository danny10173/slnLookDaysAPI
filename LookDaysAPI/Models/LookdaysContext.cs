﻿using System;
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
            entity.ToTable("ActionJoint");

            entity.Property(e => e.ActionJointId)
                .ValueGeneratedNever()
                .HasColumnName("ActionJointID");
            entity.Property(e => e.ActionTypeId).HasColumnName("ActionTypeID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ActionType).WithMany(p => p.ActionJoints)
                .HasForeignKey(d => d.ActionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActionJoint_ActionType");

            entity.HasOne(d => d.Activity).WithMany(p => p.ActionJoints)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActionJoint_Activities");

            entity.HasOne(d => d.User).WithMany(p => p.ActionJoints)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActionJoint_User");
        });

        modelBuilder.Entity<ActionType>(entity =>
        {
            entity.ToTable("ActionType");

            entity.Property(e => e.ActionTypeId)
                .ValueGeneratedNever()
                .HasColumnName("ActionTypeID");
            entity.Property(e => e.Action).HasMaxLength(50);
        });

        modelBuilder.Entity<ActivitiesAlbum>(entity =>
        {
            entity.HasKey(e => e.PhotoId);

            entity.ToTable("ActivitiesAlbum");

            entity.Property(e => e.PhotoId)
                .ValueGeneratedNever()
                .HasColumnName("PhotoID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.Photo).HasColumnType("image");

            entity.HasOne(d => d.Activity).WithMany(p => p.ActivitiesAlbums)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivitiesAlbum_Activities");
        });

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
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.City).WithMany(p => p.Activities)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activities_City");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Activities)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK_Activities_Hotels");
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

            entity.HasOne(d => d.BookingStates).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BookingStatesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_BookingStates");

            entity.HasOne(d => d.Model).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("FK_Bookings_ActivitiesModel");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_User");
        });

        modelBuilder.Entity<BookingState>(entity =>
        {
            entity.HasKey(e => e.BookingStatesId);

            entity.Property(e => e.BookingStatesId).HasColumnName("BookingStatesID");
            entity.Property(e => e.States).HasMaxLength(50);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK_Destinations");

            entity.ToTable("City");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName).HasMaxLength(50);
            entity.Property(e => e.CityPhoto).HasColumnType("image");
            entity.Property(e => e.CitySide).HasMaxLength(50);
            entity.Property(e => e.CountryId).HasColumnName("CountryID");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Country");
        });

        modelBuilder.Entity<ClassJoint>(entity =>
        {
            entity.ToTable("ClassJoint");

            entity.Property(e => e.ClassJointId)
                .ValueGeneratedNever()
                .HasColumnName("ClassJointID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");

            entity.HasOne(d => d.Activity).WithMany(p => p.ClassJoints)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassJoint_Activities");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassJoints)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassJoint_ClassName");
        });

        modelBuilder.Entity<ClassName>(entity =>
        {
            entity.HasKey(e => e.ClassId);

            entity.ToTable("ClassName");

            entity.Property(e => e.ClassId)
                .ValueGeneratedNever()
                .HasColumnName("ClassID");
            entity.Property(e => e.ClassName1)
                .HasMaxLength(50)
                .HasColumnName("ClassName");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Country1)
                .HasMaxLength(50)
                .HasColumnName("Country");
        });

        modelBuilder.Entity<CreditCardInfo>(entity =>
        {
            entity.HasKey(e => e.CinfoId);

            entity.ToTable("CreditCardInfo");

            entity.Property(e => e.CinfoId).HasColumnName("CInfoID");
            entity.Property(e => e.ExpirationDate).HasColumnType("date");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.CreditCardInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreditCardInfo_User");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.HotelPhoto).HasColumnType("image");
            entity.Property(e => e.Name).HasMaxLength(60);
        });

        modelBuilder.Entity<ModelTag>(entity =>
        {
            entity.Property(e => e.ModelTagId).HasColumnName("ModelTagID");
            entity.Property(e => e.Tags).HasMaxLength(50);
        });

        modelBuilder.Entity<ModelTagJoint>(entity =>
        {
            entity.HasKey(e => e.TagJointId);

            entity.ToTable("ModelTagJoint");

            entity.Property(e => e.TagJointId).HasColumnName("TagJointID");
            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.ModelTagId).HasColumnName("ModelTagID");

            entity.HasOne(d => d.Model).WithMany(p => p.ModelTagJoints)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ModelTagJoint_ActivitiesModel");

            entity.HasOne(d => d.ModelTag).WithMany(p => p.ModelTagJoints)
                .HasForeignKey(d => d.ModelTagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ModelTagJoint_ModelTags");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Bookings1");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.Rating).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Activity).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Activities");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.Role1)
                .HasMaxLength(50)
                .HasColumnName("Role");
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

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}