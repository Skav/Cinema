using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Models
{

    public class AppDbContext : IdentityDbContext<UsersModel>
    {
        public virtual DbSet<MoviesModel> Movies { get; set; }
        public virtual DbSet<RoomsModel> Rooms { get; set; }
        public virtual DbSet<CouponsModel> Coupons { get; set; }
        public virtual DbSet<LoyalityPointsModel> LoyalityPoints { get; set; }
        public virtual DbSet<MovieShowModel> MovieShow { get; set; }
        public virtual DbSet<ReservationModel> Reservations { get; set; }
        public virtual DbSet<ReviewsModel> Reviews { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CouponsModel>(entity =>
            {
                entity.HasOne(u => u.Users)
                .WithMany(c => c.Coupons)
                .HasForeignKey(x => x.userId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_COUPONS_USERS");
            });

            modelBuilder.Entity<LoyalityPointsModel>(entity =>
            {
                entity.HasOne(u => u.User)
                .WithMany(lp => lp.LoyalityPoints)
                .HasForeignKey(x => x.userId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_POINTS_USERS");

                entity.HasIndex(lp => lp.userId).IsUnique();
            });

            modelBuilder.Entity<ReviewsModel>(entity =>
            {
                entity.HasOne(u => u.User)
                .WithMany(r => r.Reviews)
                .HasForeignKey(x => x.userId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_REVIEWS_USERS");

                entity.HasOne(m => m.Movie)
                .WithMany(r => r.Reviews)
                .HasForeignKey(x => x.movieId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_REVIEWS_MOVIES");
            });

            modelBuilder.Entity<MovieShowModel>(entity =>
            {
                entity.HasOne(r => r.Room)
                .WithMany(ms => ms.MoviesShows)
                .HasForeignKey(x => x.roomId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_MOVIESHOW_ROOM");

                entity.HasOne(m => m.Movie)
                .WithMany(ms => ms.MoviesShows)
                .HasForeignKey(x => x.movieId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_MOVIESHOW_MOVIE");
            });

            modelBuilder.Entity<ReservationModel>(entity =>
            {
                entity.HasOne(ms => ms.MovieShow)
                .WithMany(r => r.Reservation)
                .HasForeignKey(x => x.movieShowId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_RESERVATIONS_MOVIESHOW");

                entity.HasOne(u => u.User)
                .WithMany(r => r.Reservations)
                .HasForeignKey(x => x.userId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_RESERVATION_USER");
            });
        }
    }
}
