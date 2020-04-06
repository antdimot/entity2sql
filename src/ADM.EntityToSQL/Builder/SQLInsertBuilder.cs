using System;
using System.Collections.Generic;
using System.Text;

namespace ADM.EntityToSQL.Builder
{
    public partial class SQLStatementBuilder
    {
        public string MakeInsert<T>()
        {
            var mapInfo = GetMapInfo<T>();

            var columns = ColumnsBuilder( mapInfo );

            var parameters = ParamsBuilder( mapInfo );

            return $"INSERT INTO {mapInfo.Table} ({columns}) VALUES ({parameters});";
        }
    }
}
