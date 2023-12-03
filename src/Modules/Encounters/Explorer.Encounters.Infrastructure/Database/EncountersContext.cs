﻿using Explorer.Encounters.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Encounters.Infrastructure.Database
{
    public class EncountersContext: DbContext
    {
        public DbSet<Encounter> Encounters { get; set; }

        public EncountersContext(DbContextOptions<EncountersContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("encounters");

            modelBuilder.Entity<Encounter>().ToTable("Encounters");
            modelBuilder.Entity<Encounter>().Property(x => x.Location).HasColumnType("jsonb");
            modelBuilder.Entity<Encounter>().Property(x => x.Participants).HasColumnType("jsonb");

        }

    }
}
