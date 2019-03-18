using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.Test.Entity
{
    public class Role
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class RoleMapper : ClassMapper<Role>
    {
        public RoleMapper()
        {
            Table("Role");
            AutoMap();
        }
    }
}
