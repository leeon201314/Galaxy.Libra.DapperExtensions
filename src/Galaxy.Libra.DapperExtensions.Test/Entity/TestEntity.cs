using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.Test.Entity
{
    public class TestEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TestInt { get; set; }
    }

    public class TestEntityMapper : ClassMapper<TestEntity>
    {
        public TestEntityMapper()
        {
            Table("TestTable");
            Map(p => p.Id).Key(KeyType.Identity).AutoIncrement();
            Map(p => p.TestInt).Length(4).Column("Test").Required();
            AutoMap();
        }
    }
}
