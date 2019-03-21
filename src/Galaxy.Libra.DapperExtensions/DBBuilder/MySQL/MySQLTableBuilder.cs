using Dapper;
using Galaxy.Libra.DapperExtensions.DBBuilder.MySQL.Convert;
using Galaxy.Libra.DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Galaxy.Libra.DapperExtensions.DBBuilder.MySQL
{
    public class MySQLTableBuilder : ITableBuilder
    {
        protected IDbConnection curDbConnection = null;

        private Type t;
        private IClassMapper classMap;
        private List<string> columnList = new List<string>();
        private string key;
        private string autoIncrementkey;
        private List<string> uniquekey;

        protected IDBColumnConverter DBStringConverter;
        protected IDBColumnConverter DBIntConverter;
        protected IDBColumnConverter DBLongConverter;
        protected IDBColumnConverter DBDateTimeConverter;
        protected IDBColumnConverter DBEnumConverter;
        protected IDBColumnConverter DBBoolConverter;

        public MySQLTableBuilder(IDbConnection dbConnection)
        {
            curDbConnection = dbConnection;

            DBStringConverter = new MySqlStringConverter();
            DBIntConverter = new MySQLIntConvert();
            DBLongConverter = new MySQLLongConvert();
            DBDateTimeConverter = new MySQLDateTimeConvert();
            DBEnumConverter = new MySQLEnumConvert();
            DBBoolConverter = new MySQLBoolConvert();
        }

        public void CreateTable(Type t, IClassMapper classMap)
        {
            this.t = t;
            this.classMap = classMap;
            key = "";
            autoIncrementkey = "";
            uniquekey = new List<string>();
            columnList = new List<string>();

            string sql = BuildSQL(t);

            if (curDbConnection != null)
            {
                using (curDbConnection)
                {
                    if (curDbConnection.State != ConnectionState.Open)
                        curDbConnection.Open();

                    curDbConnection.Execute(sql);

                    curDbConnection.Close();
                }
            }
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        private string GetTableName() => string.IsNullOrEmpty(classMap.TableName) ? t.Name : classMap.TableName;

        protected void InitColumns()
        {
            PropertyInfo[] properties = t.GetProperties();

            foreach (PropertyInfo pro in properties)
            {
                IPropertyMap propertyMap = classMap.Properties.SingleOrDefault(p => p.Name.Equals(pro.Name, StringComparison.InvariantCultureIgnoreCase));
                InitColumn(pro, propertyMap);
            }
        }

        protected void InitColumn(PropertyInfo pro, IPropertyMap proMap)
        {
            string columnName = GetColumnName(pro, proMap);
            StringBuilder columnStrBuilder = new StringBuilder(columnName);
            Type proType = pro.PropertyType;
            string convertStr = null;

            if (proType == typeof(string) && DBStringConverter != null)
                convertStr = DBStringConverter.Convert(proMap);
            else if (proType == typeof(int) && DBIntConverter != null)
                convertStr = DBIntConverter.Convert(proMap);
            else if (proType == typeof(long) && DBLongConverter != null)
                convertStr = DBLongConverter.Convert(proMap);
            else if (proType == typeof(DateTime) && DBDateTimeConverter != null)
                convertStr = DBDateTimeConverter.Convert(proMap);
            else if (proType == typeof(bool) && DBBoolConverter != null)
                convertStr = DBBoolConverter.Convert(proMap);
            else
                convertStr = DBStringConverter.Convert(proMap);

            columnStrBuilder.Append($" {convertStr}");

            if (proMap.IsRequired)
                columnStrBuilder.Append("  NOT NULL");

            if (proMap.IsAutoIncrement)
                columnStrBuilder.Append("  AUTO_INCREMENT");

            //if (proMap.)
            //    uniquekey.Add(columnName);

            columnList.Add(columnStrBuilder.ToString());

            if (proMap.KeyType != KeyType.NotAKey)
                key = $"PRIMARY KEY ({columnName})";
        }

        /// <summary>
        /// 获取列名称
        /// </summary>
        protected string GetColumnName(PropertyInfo pro, IPropertyMap proMap) => string.IsNullOrEmpty(proMap.ColumnName) ? pro.Name : proMap.ColumnName;


        private string BuildSQL(Type t)
        {
            this.t = t;
            this.columnList = new List<string>();
            this.key = null;
            InitColumns();

            if (!string.IsNullOrEmpty(key))
                columnList.Add(key);

            if (!string.IsNullOrEmpty(autoIncrementkey))
                columnList.Add(autoIncrementkey);

            if (uniquekey.Count > 0)
            {
                string uniquekeyStr = $"UNIQUE KEY  {string.Join('_', uniquekey.ToArray())} ({string.Join(',', uniquekey.ToArray())})";
                columnList.Add(uniquekeyStr);
            }

            StringBuilder SQLBuilder = new StringBuilder();
            SQLBuilder.Append($"CREATE TABLE If Not Exists {GetTableName()} ( ");
            SQLBuilder.Append(string.Join(",", columnList.ToArray()));
            SQLBuilder.Append(" ) ");

            return SQLBuilder.ToString();
        }
    }
}
