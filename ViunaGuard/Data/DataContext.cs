using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using ViunaGuard.Models;

namespace ViunaGuard.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {

            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            base.ConfigureConventions(builder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>().HasMany(e => e.Cars)
                           .WithMany(e => e.People);

            modelBuilder.Entity<Person>().HasIndex(p => p.NationalId).IsUnique();

            modelBuilder.Entity<Organization>()
                .HasMany(org => org.AreBanned)
                .WithMany(p => p.BannedFrom)
                .UsingEntity(join => join.ToTable("BlackList"));
        }

        public DbSet<Person> People => Set<Person>();
        public DbSet<Organization> Organizations=> Set<Organization>();
        public DbSet<PersonAdditionalInfo> PersonAdditionalInfos=> Set<PersonAdditionalInfo>();
        public DbSet<Car> Cars=> Set<Car>();
        public DbSet<Door> Doors=> Set<Door>();
        public DbSet<Employee> Employees=> Set<Employee>();
        public DbSet<Entrance> Entrances=> Set<Entrance>();
        public DbSet<EntrancePermission> EntrancePermissions=> Set<EntrancePermission>();
        //public DbSet<BlackList> BlackList => Set<BlackList>();
        public DbSet<EmployeeShift> EmployeeShifts => Set<EmployeeShift>();
        public DbSet<EntranceGroup> EntranceGroups => Set<EntranceGroup>();
        public DbSet<EntrancePolicie> EntrancePolicies => Set<EntrancePolicie>();
        public DbSet<OrganizationPolicie> OrganizationPolicies => Set<OrganizationPolicie>();
        public DbSet<UserAccess> UserAccesses => Set<UserAccess>();
        public DbSet<Authority> Authorities => Set<Authority>();
        public DbSet<EmployeeShiftPeriodicMonthly> EmployeeShiftsMonthly => Set<EmployeeShiftPeriodicMonthly>();
        public DbSet<EmployeeShiftPeriodicWeekly> EmployeeShiftsWeekly => Set<EmployeeShiftPeriodicWeekly>();
    }
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter()
            : base(dateOnly =>
                    dateOnly.ToDateTime(TimeOnly.MinValue),
                dateTime => DateOnly.FromDateTime(dateTime))
        { }
    }
}
