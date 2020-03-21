using HMA2.Models;
using Microsoft.EntityFrameworkCore;

namespace HMA2.Data
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Version> Version { get; set; }

        public virtual DbSet<VersionType> VersionType { get; set; }

        public virtual DbSet<File> File { get; set; }

        public virtual DbSet<VersionFileMapping> VersionFileMapping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}