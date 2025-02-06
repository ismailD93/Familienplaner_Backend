using Castle.Components.DictionaryAdapter.Xml;
using KalenderAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace KalenderAppBackend.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvent { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // UserEvent zusammengesetzten Schlüssel und Fremdschlüsselbeziehungen
            builder.Entity<UserEvent>(x => x.HasKey(e => new { e.UserId, e.EventId }));

            builder.Entity<UserEvent>()
                .HasOne(u => u.User)
                .WithMany(u => u.UserEvents)
                .HasForeignKey(u => u.UserId);

            builder.Entity<UserEvent>()
                .HasOne(e => e.Event)
                .WithMany(e => e.UserEvents)
                .HasForeignKey(e => e.EventId);
        }
    }
}
