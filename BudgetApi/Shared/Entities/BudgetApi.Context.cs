﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BudgetApi.Models
{
    using BudgetApi.Shared;
    using Microsoft.EntityFrameworkCore;

    public partial class BudgetEntities : DbContext
    {
        public BudgetEntities(DbContextOptions<BudgetEntities> options) : base(options)
        {
        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Budget>().ToTable("Budget");
            modelBuilder.Entity<BudgetTypeEntity>().ToTable("BudgetType");
            modelBuilder.Entity<GiftCard>().ToTable("GiftCard");
            modelBuilder.Entity<Income>().ToTable("Income");
            modelBuilder
                .Entity<IncomeSourceEntity>()
                .ToTable("IncomeSource");
                //.HasKey(source => source.Id);

            modelBuilder.Entity<Purchase>().ToTable("Purchases");
            modelBuilder.Entity<CustomSettings>().ToTable("Settings");
        }
    
        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<BudgetTypeEntity> BudgetTypes { get; set; }
        public virtual DbSet<GiftCard> GiftCards { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<IncomeSourceEntity> IncomeSources { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<CustomSettings> Settings { get; set; }
    }
}
