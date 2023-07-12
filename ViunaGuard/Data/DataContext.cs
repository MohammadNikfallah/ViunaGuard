using Microsoft.EntityFrameworkCore;
using ViunaGuard.Models;

namespace ViunaGuard.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Person> People => Set<Person>();
        public DbSet<Organization> Organizations=> Set<Organization>();
        public DbSet<AdditionalInfo> AdditionalInfos=> Set<AdditionalInfo>();
        public DbSet<Car> Cars=> Set<Car>();
        public DbSet<Door> Doors=> Set<Door>();
        public DbSet<Employee> Employees=> Set<Employee>();
        public DbSet<Entrance> Entrances=> Set<Entrance>();
        public DbSet<EntrancePermission> EntrancePermissions=> Set<EntrancePermission>();
    }
}
