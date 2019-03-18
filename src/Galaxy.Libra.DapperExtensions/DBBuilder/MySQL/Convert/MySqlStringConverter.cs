using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.DBBuilder.MySQL.Convert
{
    public class MySqlStringConverter : IDBColumnConverter
    {
        public string Convert(IPropertyMap propertyMap)
        {
            if (propertyMap != null)
            {
                if (propertyMap.ColumnLength == int.MaxValue)
                    return "LONGTEXT";
                else if (propertyMap.ColumnLength <= 0)
                    return "varchar(255)";
                else
                    return $"varchar({propertyMap.ColumnLength})";
            }

            return "varchar(255)";
        }
    }
}