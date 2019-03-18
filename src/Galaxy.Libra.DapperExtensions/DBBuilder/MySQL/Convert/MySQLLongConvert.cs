using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.DBBuilder.MySQL.Convert
{
    public class MySQLLongConvert : IDBColumnConverter
    {
        public string Convert(IPropertyMap propertyMap) => "bigint";
    }
}
