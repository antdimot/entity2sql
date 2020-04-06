
using System;
using System.Linq.Expressions;

namespace ADM.EntityToSQL.Builder
{
    public partial class SQLStatementBuilder
    {
        // create select command for entity
        public string MakeSelect<T>( Expression<Func<T, bool>> predicate = null )
        {
            var mapInfo = GetMapInfo<T>();

            var columns = ColumnsBuilder( mapInfo );

            if( predicate is null )
            {
                return $"SELECT {columns} FROM {mapInfo.Table}";
            }
            else
            {
                var condition = EvaluatePredicate<T>( predicate );

                return $"SELECT {columns} FROM {mapInfo.Table} WHERE {condition}";
            }
        }

        public string MakeSelectCustomWhere<T>( string customWhere )
        {           
            return $"{MakeSelect<T>()} WHERE {customWhere}";
        }

        private string EvaluatePredicate<T>( Expression expression )
        {
            switch( expression.NodeType )
            {
                case ExpressionType.Constant:
                    var cexp = expression as ConstantExpression;

                    if( cexp.Type.Name == "String" )
                    {
                        return $"'{cexp.Value}'";
                    }

                    return $"{cexp.Value}";
                case ExpressionType.Equal:
                    var bexp = expression as BinaryExpression;

                    var leftExpression = EvaluatePredicate<T>( bexp.Left );
                    var rightExpression = EvaluatePredicate<T>( bexp.Right );
                    
                    return $"{leftExpression} = {rightExpression}";
                case ExpressionType.Lambda:
                    var lexp = expression as LambdaExpression;

                    return EvaluatePredicate<T>( lexp.Body );
                case ExpressionType.MemberAccess:
                    var mexp = expression as MemberExpression;

                    var columnName = this.GetMapInfo<T>().ColumnsDictionary [mexp.Member.Name];

                    return $"{columnName}";              
                default:
                    throw new ArgumentException("The expression is not supported."); 
            }
        }
    }
}
