using Meetings.Domain;
using Microsoft.EntityFrameworkCore;
using MassTransit;


namespace Meetings.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddOutboxMessageEntity(); //Table of messages before sent to messageBroker
            modelBuilder.AddOutboxStateEntity(); // Table for status of messages at outbox table & concurrent processors
            modelBuilder.AddInboxStateEntity(); // Table for status of messages at inbox table & concurrent processors
       
        }
    }
}
