using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.DBBuilder.MySQL.Convert
{
    public class MySQLBoolConvert : IDBColumnConverter
    {
        public string Convert(IPropertyMap propertyMap) => "bool";
    }
}
