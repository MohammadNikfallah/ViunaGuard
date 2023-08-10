using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using ViunaGuard.Models;
using ViunaGuard.Models.Enums;

namespace ViunaGuard.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            
        }
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

        public DbSet<Person> People { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<PersonAdditionalInfo> PersonAdditionalInfos { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Door> Doors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Entrance> Entrances { get; set; }
        public DbSet<EntrancePermission> EntrancePermissions { get; set; }
        public DbSet<EmployeeShift> EmployeeShifts { get; set; }
        public DbSet<EntranceGroup> EntranceGroups { get; set; }
        public DbSet<EntrancePolicie> EntrancePolicies { get; set; }
        public DbSet<OrganizationPolicie> OrganizationPolicies { get; set; }
        public DbSet<UserAccess> UserAccesses { get; set; }
        public DbSet<Authority> Authorities { get; set; }
        public DbSet<EmployeeShiftPeriodicMonthly> EmployeeShiftsMonthly { get; set; }
        public DbSet<EmployeeShiftPeriodicWeekly> EmployeeShiftsWeekly { get; set; }
        public DbSet<SignatureNeedForEntrancePermission> SignatureNeedForEntrancePermissions { get; set; }
        public DbSet<SignedEntrancePermission> SignedEntrancePermissions { get; set; }
        public DbSet<AuthIdToViunaId> AuthIds { get; set; }


        public DbSet<CarBrand> CarBrands {  get; set; }
        public DbSet<CarModel> CarModels {  get; set; }
        public DbSet<City> Cities {  get; set; }
        public DbSet<Color> Colors{  get; set; }
        public DbSet<EducationalDegree> EducationalDegrees {  get; set; }
        public DbSet<EmployeeType> EmployeeTypes {  get; set; }
        public DbSet<EnterOrExit> EnterOrExit {  get; set; }
        public DbSet<EntranceType> EntranceTypes {  get; set; }
        public DbSet<Gender> Genders {  get; set; }
        public DbSet<MaritalStatus> MaritalStatuses {  get; set; }
        public DbSet<MilitaryServiceStatus> MilitaryServiceStatuses {  get; set; }
        public DbSet<Nationality> Nationalities {  get; set; }
        public DbSet<Religion> Religions {  get; set; }
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
