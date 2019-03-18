using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.DBBuilder.MySQL.Convert
{
    public class MySQLEnumConvert : IDBColumnConverter
    {
        public string Convert(IPropertyMap propertyMap) => "int";
    }
}
