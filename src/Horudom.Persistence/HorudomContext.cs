using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Horudom.Models;

using Microsoft.EntityFrameworkCore;

namespace Horudom.Data
{
	public class HorudomContext : DbContext
	{
		public HorudomContext(DbContextOptions<HorudomContext> opt) : base(opt)
		{

		}

		public DbSet<Actor> Actors { get; set; }
		public DbSet<Director> Directors { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<MovieActor> MovieActors { get; set; }
		public DbSet<MovieDirector> MovieDirectors { get; set; }
		public DbSet<MovieGenre> MovieGenres { get; set; }
		public DbSet<MovieWriter> MovieWriters { get; set; }
		public DbSet<Poster> Posters { get; set; }
		public DbSet<Screenshot> Screenshots { get; set; }
		public DbSet<Writer> Writers { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<MovieActor>(e =>
			{
				e.HasOne(mv => mv.Movie)
					.WithMany()
					.HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
				e.HasOne(mv => mv.Actor)
					.WithMany()
					.HasForeignKey("ActorId").OnDelete(DeleteBehavior.Restrict);
				e.HasKey("MovieId", "ActorId");
			});

			modelBuilder.Entity<MovieDirector>(e =>
			{
				e.HasOne(mv => mv.Movie)
					.WithMany()
					.HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
				e.HasOne(mv => mv.Director)
					.WithMany()
					.HasForeignKey("DirectorId").OnDelete(DeleteBehavior.Restrict);
				e.HasKey("MovieId", "DirectorId");
			});

			modelBuilder.Entity<MovieWriter>(e =>
			{
				e.HasOne(mv => mv.Movie)
					.WithMany()
					.HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
				e.HasOne(mv => mv.Writer)
					.WithMany()
					.HasForeignKey("WriterId").OnDelete(DeleteBehavior.Restrict);
				e.HasKey("MovieId", "WriterId");
			});

			modelBuilder.Entity<MovieGenre>(e =>
			{
				e.HasOne(mv => mv.Movie)
					.WithMany()
					.HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict);
				e.HasOne(mv => mv.Genre)
					.WithMany()
					.HasForeignKey("GenreId").OnDelete(DeleteBehavior.Restrict);
				e.HasKey("MovieId", "GenreId");
			});

			modelBuilder.Entity<Screenshot>(e =>
			{
				e.HasOne(mv => mv.Movie).WithMany().OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<Poster>(e =>
			{
				e.HasOne(mv => mv.Movie).WithMany().OnDelete(DeleteBehavior.Restrict);
			});


		}

	}
}
