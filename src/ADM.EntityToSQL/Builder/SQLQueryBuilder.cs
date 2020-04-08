
using ADM.EntityToSQL.Meta;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace ADM.EntityToSQL.Builder
{
    public partial class SQLStatementBuilder
    {
        public string MakeSelect<T>( Expression<Func<T, bool>> predicate = null )
        {
            var mapInfo = GetMapInfo<T>();

            var columns = ColumnsBuilder( mapInfo );

            if( predicate is null )
            {
                return $"SELECT {columns} FROM {mapInfo.Table} {mapInfo.Alias}";
            }
            else
            {
                var condition = EvaluatePredicate<T>( predicate );

                return $"SELECT {columns} FROM {mapInfo.Table} {mapInfo.Alias} WHERE {condition}";
            }
        }

        private string EvaluatePredicate<T>( Expression expression, bool forQuery = true )
        {
            switch( expression.NodeType )
            {
                case ExpressionType.OrElse:
                    var oreExp = expression as BinaryExpression;

                    var orLeftExpResult = EvaluatePredicate<T>( oreExp.Left, forQuery );
                    var orRightExpResult = EvaluatePredicate<T>( oreExp.Right, forQuery );

                    return $"({orLeftExpResult} OR {orRightExpResult})";
                case ExpressionType.AndAlso:
                    var andExp = expression as BinaryExpression;

                    var andLeftExpResult = EvaluatePredicate<T>( andExp.Left, forQuery );
                    var andRightExpResult = EvaluatePredicate<T>( andExp.Right, forQuery );

                    return $"({andLeftExpResult} AND {andRightExpResult})";
                case ExpressionType.Constant:
                    var cexp = expression as ConstantExpression;

                    if( cexp.Type.Name == "String" )
                        return $"'{cexp.Value}'";

                    return $"{cexp.Value}";
                case ExpressionType.Equal:
                    var bexp = expression as BinaryExpression;

                    var leftExp = EvaluatePredicate<T>( bexp.Left, forQuery );
                    var rightExp = EvaluatePredicate<T>( bexp.Right, forQuery );               

                    return $"{leftExp} = {rightExp}";
                case ExpressionType.Lambda:
                    var lexp = expression as LambdaExpression;

                    return EvaluatePredicate<T>( lexp.Body, forQuery );
                case ExpressionType.MemberAccess:
                    var mexp = expression as MemberExpression;

                    var minfo = GetMapInfo<T>();

                    if( forQuery )
                    {
                        return $"{minfo.Alias}.{minfo.ColumnsDictionary[mexp.Member.Name]}";
                    }
                    else
                    {
                        return $"{minfo.ColumnsDictionary[mexp.Member.Name]}";
                    }
                default:
                    throw new ArgumentException("The expression is not supported.");
            }
        }

        public string MakeJoin<T, K>( Expression<Func<T, K, bool>> predicate )
        {
            var mapInfo1 = GetMapInfo<T>();
            var mapInfo2 = GetMapInfo<K>();

            if( !(predicate.Body.NodeType == ExpressionType.Equal) )
                throw new ArgumentException("The predicate should be a join property condition.");

            var bexp = predicate.Body as BinaryExpression;

            var column1 = mapInfo1.ColumnsDictionary[typeof( K ).Name];
            var column2 = EvaluatePredicate<K>( bexp.Right );

            return $"SELECT {ColumnsBuilder(mapInfo1)},{ColumnsBuilder(mapInfo2)} FROM {mapInfo1.Table} {mapInfo1.Alias} " +
                   $"INNER JOIN {mapInfo2.Table} {mapInfo2.Alias} ON {mapInfo1.Alias}.{column1}={mapInfo2.Alias}.{column2}";
        }
    }
}
