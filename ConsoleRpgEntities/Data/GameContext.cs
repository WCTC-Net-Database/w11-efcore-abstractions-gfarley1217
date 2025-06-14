﻿using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using Microsoft.EntityFrameworkCore;
using ConsoleRpgEntities.Models;

namespace ConsoleRpgEntities.Data
{
    public class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Monster> Monsters { get; set; } = null!;
        public DbSet<Ability> Abilities { get; set; } = null!;
        public DbSet<Equipment> Equipments { get; set; } = null!;

        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TPH for Character hierarchy
            modelBuilder.Entity<Monster>()
                .HasDiscriminator<string>(m => m.MonsterType)
                .HasValue<Goblin>("Goblin");

            // Configure TPH for Ability hierarchy
            modelBuilder.Entity<Ability>()
                .HasDiscriminator<string>(pa => pa.AbilityType)
                .HasValue<ShoveAbility>("ShoveAbility");

            // Configure many-to-many relationship between Player and Abilities
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Abilities)
                .WithMany(a => a.Players)
                .UsingEntity(j => j.ToTable("PlayerAbilities"));

            // Configure one-to-one relationship between Player and Equipment
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Equipment)
                .WithOne()
                .HasForeignKey<Equipment>(e => e.Id) // Assuming Equipment.Id is the foreign key
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a player is deleted

            base.OnModelCreating(modelBuilder);
        }
    }
}




