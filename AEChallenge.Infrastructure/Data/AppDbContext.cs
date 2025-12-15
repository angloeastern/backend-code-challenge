
using Microsoft.EntityFrameworkCore;
using AEChallenge.Domain.Entities;
using AEChallenge.Application.DTOs.Response; // Read Models

namespace AEChallenge.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Ship> Ships { get; set; } = default!;
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; } = default!;
        public DbSet<FiscalPeriod> FiscalPeriods { get; set; } = default!;
        public DbSet<ActualExpense> ActualExpenses { get; set; } = default!;
        public DbSet<Budget> Budgets { get; set; } = default!;
        public DbSet<SystemUser> SystemUsers { get; set; } = default!;
        public DbSet<UserShipAssignment> UserShipAssignments { get; set; } = default!;
        public DbSet<CrewMember> CrewMembers { get; set; } = default!;
        public DbSet<Rank> Ranks { get; set; } = default!;
        public DbSet<CrewServiceHistory> CrewServiceHistories { get; set; } = default!;





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<UserShipAssignment>()
                .HasKey(a => new { a.UserId, a.ShipCode });


            modelBuilder.Entity<ActualExpense>()
                .HasIndex(a => new { a.ShipCode, a.COA_ID, a.PeriodID })
                .IsUnique()
                .HasDatabaseName("UQ_Actual");


            modelBuilder.Entity<Budget>()
                .HasIndex(b => new { b.ShipCode, b.COA_ID, b.PeriodID })
                .IsUnique()
                .HasDatabaseName("UQ_Budget");


            modelBuilder.Entity<ChartOfAccount>()
                .HasIndex(c => c.AccountNumber)
                .IsUnique();


            modelBuilder.Entity<FiscalPeriod>()
                .HasIndex(p => new { p.FiscalYear, p.PeriodMonth })
                .IsUnique()
                .HasDatabaseName("UQ_FiscalPeriod");


            modelBuilder.Entity<Rank>()
                .HasIndex(r => r.RankName)
                .IsUnique();


            modelBuilder.Entity<SystemUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();




            modelBuilder.Entity<ChartOfAccount>()
                .HasOne(c => c.ParentAccount)
                .WithMany(c => c.ChildAccounts)
                .HasForeignKey(c => c.ParentCOA_ID)
                .IsRequired(false); // ParentCOA_ID is NULLable

            ConfigureReadModels(modelBuilder);
        }
        private static void ConfigureReadModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CrewListReadModel>().ToView(null);
            modelBuilder.Entity<FinancialReportSummaryReadModel>().ToView(null);
            modelBuilder.Entity<FinancialReportDetailReadModel>().ToView(null);
            modelBuilder.Entity<UserLoginReadModel>().ToView(null);
            modelBuilder.Entity<UserCreationReadModel>().ToView(null);
            modelBuilder.Entity<ShipReadModel>().ToView(null);
            modelBuilder.Entity<ShipAssignmentReadModel>().ToView(null);

        }
    }
}