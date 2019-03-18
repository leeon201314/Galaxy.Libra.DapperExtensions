using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.Test.Entity
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Psw { get; set; }

        public string RoleId { get; set; }

        public Role Role { get; set; }
    }

    public class UserMapper : ClassMapper<User>
    {
        public UserMapper()
        {
            Table("UserTest");
            Map(p => p.Id).Key(KeyType.Assigned);
            Map(p => p.Role).Ignore();
            AutoMap();
        }
    }
}
