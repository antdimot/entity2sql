using ADM.EntityToSQL.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADM.EntityToSQL.Builder
{
    public partial class SQLStatementBuilder
    {
        // caching entity map metadata
        private IDictionary<string,EntityInfo> _mapInfo;

        public SQLStatementBuilder()
        {
            _mapInfo = new Dictionary<string, EntityInfo>();
        }

        // manages entity map metadata
        private EntityInfo GetMapInfo<T>()
        {
            if( !_mapInfo.ContainsKey( typeof(T).Name ) )
            {
                _mapInfo.Add( typeof( T ).Name, EntityInfo.BuildMap<T>() );
            }

            return _mapInfo[typeof( T ).Name];
        }

        private string ColumnsBuilder( EntityInfo entityInfo )
        {
            var columnBuilder = new StringBuilder();

            foreach( var item in entityInfo.Columns )
            {
                columnBuilder.AppendFormat( "{0},", item );
            }

            columnBuilder.Remove( columnBuilder.Length - 1, 1 );

            return columnBuilder.ToString();
        }

        private string ParamsBuilder( EntityInfo entityInfo )
        {
            var paramBuilder = new StringBuilder();

            foreach( var item in entityInfo.Columns )
            {
                if( !entityInfo.Keys.Contains( item ) )
                {
                    paramBuilder.AppendFormat( "@{0},", item );
                }
            }

            paramBuilder.Remove( paramBuilder.Length - 1, 1 );

            return paramBuilder.ToString();
        }

        //private string KeysColumnsBuilder( EntityInfo entityInfo )
        //{
        //    var keyColumnBuilder = new StringBuilder();

        //    foreach( var item in entityInfo.Keys )
        //    {
        //        keyColumnBuilder.Append( $"{item}=@{item} AND " );
        //    }

        //    keyColumnBuilder.Remove( keyColumnBuilder.Length - 5, 5 );

        //    return keyColumnBuilder.ToString();
        //}

        public object GetValue<T>( string properyName, T obj )
        {
            return typeof( T ).GetProperty( properyName )
                              .GetValue( obj );
        }
    }
}
