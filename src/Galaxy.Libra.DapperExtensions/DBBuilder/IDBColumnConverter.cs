using Galaxy.Libra.DapperExtensions.Mapper;

namespace Galaxy.Libra.DapperExtensions.DBBuilder
{
    public interface IDBColumnConverter
    {
        string Convert(IPropertyMap propertyMap);
    }
}
