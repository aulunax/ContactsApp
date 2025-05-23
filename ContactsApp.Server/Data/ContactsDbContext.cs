﻿using ContactsApp.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace ContactsApp.Server.Data
{

    /// <summary>
    /// Entity Framework Core DbContext for contacts and users.
    /// </summary>
    public class ContactsDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // User setup

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Contacts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Contact and Categories setup

            modelBuilder.Entity<Contact>()
                .HasIndex(u => u.Email);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .IsRequired(false);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Subcategory)
                .WithMany()
                .HasForeignKey(c => c.SubcategoryId)
                .IsRequired(false);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Subcategories)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .IsRequired();


            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Work" },
                new Category { Id = 2, Name = "Private" },
                new Category { Id = 3, Name = "Other" }
            );

            modelBuilder.Entity<Subcategory>().HasData(
                new Subcategory { Id = 1, Name = "Boss", CategoryId = 1 },
                new Subcategory { Id = 2, Name = "Client", CategoryId = 1 }
            );

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=contacts.db");

    }
}
