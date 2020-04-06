
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
                case ExpressionType.OrElse:
                    var oreExp = expression as BinaryExpression;

                    var orLeftExpResult = EvaluatePredicate<T>( oreExp.Left );
                    var orRightExpResult = EvaluatePredicate<T>( oreExp.Right );

                    return $"({orLeftExpResult} OR {orRightExpResult})";
                case ExpressionType.AndAlso:
                    var andExp = expression as BinaryExpression;

                    var andLeftExpResult = EvaluatePredicate<T>( andExp.Left );
                    var andRightExpResult = EvaluatePredicate<T>( andExp.Right );

                    return $"({andLeftExpResult} AND {andRightExpResult})";
                case ExpressionType.Constant:
                    var cexp = expression as ConstantExpression;

                    if( cexp.Type.Name == "String" )
                        return $"'{cexp.Value}'";

                    return $"{cexp.Value}";
                case ExpressionType.Equal:
                    var bexp = expression as BinaryExpression;

                    var leftExp = EvaluatePredicate<T>( bexp.Left );
                    var rightExp = EvaluatePredicate<T>( bexp.Right );
                    
                    return $"{leftExp} = {rightExp}";
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
