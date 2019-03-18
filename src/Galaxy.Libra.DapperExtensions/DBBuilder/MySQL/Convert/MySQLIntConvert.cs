using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.DBBuilder.MySQL.Convert
{
    public class MySQLIntConvert : IDBColumnConverter
    {
        public string Convert(IPropertyMap propertyMap)
        {
            if (propertyMap != null)
            {
                if (propertyMap.ColumnLength <= 0)
                    return "int";
                else
                    return $"int({propertyMap.ColumnLength})";
            }

            return "int";
        }
    }
}
