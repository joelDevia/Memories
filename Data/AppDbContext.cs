using Memories.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Memories.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Memory> Memories { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaFile>()
                .HasOne(m => m.Memory)
                .WithMany(mem => mem.MediaFiles)
                .HasForeignKey(m => m.MemoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
