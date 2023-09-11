using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

            modelBuilder.Entity<Person>()
                .HasMany(e => e.Cars)
                .WithMany(e => e.People);

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.NationalId)
                .IsUnique();

            modelBuilder.Entity<Organization>()
                .HasMany(org => org.AreBanned)
                .WithMany(p => p.BannedFrom)
                .UsingEntity(join => join.ToTable("BlackList"));

            modelBuilder.Entity<Car>()
                .HasIndex(c => c.LicenseNumber)
                .IsUnique();
        }

        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<PersonAdditionalInfo> PersonAdditionalInfos { get; set; } = null!;
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Door> Doors { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Entrance> Entrances { get; set; } = null!;
        public DbSet<EntrancePermission> EntrancePermissions { get; set; } = null!;
        public DbSet<EmployeeShift> EmployeeShifts { get; set; } = null!;
        public DbSet<EntranceGroup> EntranceGroups { get; set; } = null!;
        public DbSet<EntrancePolicie> EntrancePolicies { get; set; } = null!;
        public DbSet<OrganizationPolicy> OrganizationPolicies { get; set; } = null!;
        public DbSet<UserAccess> UserAccesses { get; set; } = null!;
        public DbSet<EmployeePeriodicShift> EmployeePeriodicShifts { get; set; } = null!;
        public DbSet<SignatureNeedForEntrancePermission> SignatureNeedForEntrancePermissions { get; set; } = null!;
        public DbSet<SignedEntrancePermission> SignedEntrancePermissions { get; set; } = null!;
        public DbSet<EmployeeShiftPeriodicMonthly> EmployeeShiftsPeriodicMonthly { get; set; } = null!;
        public DbSet<UserAccessRole> UserAccessRole { get; set; } = null!;


        public DbSet<CarBrand> CarBrands {  get; set; } = null!;
        public DbSet<CarModel> CarModels {  get; set; } = null!;
        public DbSet<City> Cities {  get; set; } = null!;
        public DbSet<Color> Colors{  get; set; } = null!;
        public DbSet<EducationalDegree> EducationalDegrees {  get; set; } = null!;
        public DbSet<EmployeeType> EmployeeTypes {  get; set; } = null!;
        public DbSet<EnterOrExit> EnterOrExit {  get; set; } = null!;
        public DbSet<Gender> Genders {  get; set; } = null!;
        public DbSet<MaritalStatus> MaritalStatuses {  get; set; } = null!;
        public DbSet<MilitaryServiceStatus> MilitaryServiceStatuses {  get; set; } = null!;
        public DbSet<Nationality> Nationalities {  get; set; } = null!;
        public DbSet<Religion> Religions {  get; set; } = null!;
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
