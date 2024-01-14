﻿// <auto-generated />
using System;
using Cinema.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinema.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Cinema.Models.CouponsModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("dateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("discount")
                        .HasColumnType("integer");

                    b.Property<string>("discountType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("expDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("Cinema.Models.LoyalityPointsModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("amountOfPoints")
                        .HasColumnType("integer");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("dateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("userId")
                        .IsUnique();

                    b.ToTable("LoyalityPoints");
                });

            modelBuilder.Entity("Cinema.Models.MovieShowModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("dateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("hour")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("movieId")
                        .HasColumnType("integer");

                    b.Property<int>("roomId")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("movieId");

                    b.HasIndex("roomId");

                    b.ToTable("MovieShow");
                });

            modelBuilder.Entity("Cinema.Models.MoviesModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<bool>("available")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("dateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("duration")
                        .HasColumnType("integer");

                    b.Property<string>("genre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Cinema.Models.ReservationModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime>("claimDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("dateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("fullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isPaid")
                        .HasColumnType("boolean");

                    b.Property<int>("movieShowId")
                        .HasColumnType("integer");

                    b.Property<int>("seatColumn")
                        .HasColumnType("integer");

                    b.Property<int>("seatRow")
                        .HasColumnType("integer");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("movieShowId");

                    b.HasIndex("userId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Cinema.Models.ReviewsModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("dateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("movieId")
                        .HasColumnType("integer");

                    b.Property<int>("rating")
                        .HasColumnType("integer");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("movieId");

                    b.HasIndex("userId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Cinema.Models.RoomsModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("columns")
                        .HasColumnType("integer");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("dateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("roomNo")
                        .HasColumnType("integer");

                    b.Property<int>("rows")
                        .HasColumnType("integer");

                    b.Property<int>("seatsInRow")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Cinema.Models.UsersModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Cinema.Models.CouponsModel", b =>
                {
                    b.HasOne("Cinema.Models.UsersModel", "Users")
                        .WithMany("Coupons")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_COUPONS_USERS");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Cinema.Models.LoyalityPointsModel", b =>
                {
                    b.HasOne("Cinema.Models.UsersModel", "User")
                        .WithMany("LoyalityPoints")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_POINTS_USERS");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cinema.Models.MovieShowModel", b =>
                {
                    b.HasOne("Cinema.Models.MoviesModel", "Movie")
                        .WithMany("MoviesShows")
                        .HasForeignKey("movieId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_MOVIESHOW_MOVIE");

                    b.HasOne("Cinema.Models.RoomsModel", "Room")
                        .WithMany("MoviesShows")
                        .HasForeignKey("roomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_MOVIESHOW_ROOM");

                    b.Navigation("Movie");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Cinema.Models.ReservationModel", b =>
                {
                    b.HasOne("Cinema.Models.MovieShowModel", "MovieShow")
                        .WithMany("Reservation")
                        .HasForeignKey("movieShowId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_RESERVATIONS_MOVIESHOW");

                    b.HasOne("Cinema.Models.UsersModel", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_RESERVATION_USER");

                    b.Navigation("MovieShow");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cinema.Models.ReviewsModel", b =>
                {
                    b.HasOne("Cinema.Models.MoviesModel", "Movie")
                        .WithMany("Reviews")
                        .HasForeignKey("movieId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_REVIEWS_MOVIES");

                    b.HasOne("Cinema.Models.UsersModel", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_REVIEWS_USERS");

                    b.Navigation("Movie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Cinema.Models.UsersModel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Cinema.Models.UsersModel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cinema.Models.UsersModel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Cinema.Models.UsersModel", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cinema.Models.MovieShowModel", b =>
                {
                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("Cinema.Models.MoviesModel", b =>
                {
                    b.Navigation("MoviesShows");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Cinema.Models.RoomsModel", b =>
                {
                    b.Navigation("MoviesShows");
                });

            modelBuilder.Entity("Cinema.Models.UsersModel", b =>
                {
                    b.Navigation("Coupons");

                    b.Navigation("LoyalityPoints");

                    b.Navigation("Reservations");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
