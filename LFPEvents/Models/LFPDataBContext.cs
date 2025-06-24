using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace LFPEvents.Models
{
    public partial class LFPDataBContext : DbContext
    {
        public LFPDataBContext()
            : base("name=LFPDataBContext1")
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public DbSet<EventType> EventTypes { get; set; }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<Event>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Event>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.Event)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.ImageURL)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .HasMany(e => e.Events)
                .WithRequired(e => e.Venue)
                .WillCascadeOnDelete(false);
        }
    }
}
