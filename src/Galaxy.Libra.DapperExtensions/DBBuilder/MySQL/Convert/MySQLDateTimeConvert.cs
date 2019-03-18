using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.DBBuilder.MySQL.Convert
{
    public class MySQLDateTimeConvert : IDBColumnConverter
    {
        public string Convert(IPropertyMap propertyMap) => "datetime";
    }
}
