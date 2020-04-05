using ADM.EntityToSQL.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADM.EntityToSQL
{
    public sealed class SQLCommandBuilder
    {
        // caching entity map metadata
        IDictionary<string,EntityInfo> _mapInfo;

        public SQLCommandBuilder()
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

        private string KeysColumnsBuilder( EntityInfo entityInfo )
        {
            var keyColumnBuilder = new StringBuilder();

            foreach( var item in entityInfo.Keys )
            {
                keyColumnBuilder.Append( $"{item}=@{item} AND " );
            }

            keyColumnBuilder.Remove( keyColumnBuilder.Length - 5, 5 );

            return keyColumnBuilder.ToString();
        }

        public string MakeInsertCommand<T>()
        {
            var mapInfo = GetMapInfo<T>();

            var columns = ColumnsBuilder( mapInfo );
            var parameters = ParamsBuilder( mapInfo );

            return $"INSERT INTO {mapInfo.Table} ({columns}) VALUES ({parameters});";
        }

        // create select command for entity
        public string MakeSelectCommand<T>()
        {
            var mapInfo = GetMapInfo<T>();

            var columns = ColumnsBuilder( mapInfo );

            return $"SELECT {columns} FROM {mapInfo.Table}";
        }

        // create select command with key in where condition
        public string MakeSelectCommandWithKeys<T>()
        {
            var mapInfo = GetMapInfo<T>();

            var query = MakeSelectCommand<T>();

            var keys = ColumnsBuilder( mapInfo );

            return $"{query} WHERE {keys}";
        }

        public string MakeSelectCommandCustom<T>( string customWhere )
        {
            var mapInfo = GetMapInfo<T>();

            var query = MakeSelectCommand<T>();

            return $"{query} WHERE {customWhere}";
        }

        public object GetValue<T>( string properyName, T obj )
        {
            return typeof( T ).GetProperty( properyName )
                              .GetValue( obj );
        }
    }
}
