using Galaxy.Libra.DapperExtensions.Mapper;
using System;

namespace Galaxy.Libra.DapperExtensions.DBBuilder
{
    public interface ITableBuilder
    {
        void CreateTable(Type t, IClassMapper classMap);
    }
}
