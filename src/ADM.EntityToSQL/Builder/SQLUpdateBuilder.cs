using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ADM.EntityToSQL.Builder
{
    public partial class SQLStatementBuilder
    {
        public string MakeUpdate<T>( Expression<Func<T, bool>> predicate = null )
        {
            var mapInfo = GetMapInfo<T>();

            var parameters = ParamsBuilder( mapInfo, false );

            if( predicate is null )
            {
                return $"UPDATE {mapInfo.Table} SET {parameters}";
            }
            else
            {
                var condition = EvaluatePredicate<T>( predicate, false );

                return $"UPDATE {mapInfo.Table} SET {parameters} WHERE {condition}";
            }
        }
    }
}
