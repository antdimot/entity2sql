using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ADM.EntityToSQL.Builder
{
    public partial class SQLStatementBuilder
    {
        public string MakeDelete<T>( Expression<Func<T, bool>> predicate = null )
        {
            var mapInfo = GetMapInfo<T>();

            if( predicate is null )
            {
                return $"DELETE FROM {mapInfo.Table}";
            }
            else
            {
                var condition = EvaluatePredicate<T>( predicate, false );

                return $"DELETE FROM {mapInfo.Table} WHERE {condition}";
            }
        }
    }
}
