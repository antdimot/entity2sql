using ADM.EntityToSQL.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADM.EntityToSQL.Builder
{
    public partial class SQLStatementBuilder
    {
        private int _counter = 0;

        private int NextCounter()
        {
            return _counter += 1;
        }

        // caching entity map metadata
        private IDictionary<string,EntityInfo> _mapInfo;

        public SQLStatementBuilder()
        {
            _mapInfo = new Dictionary<string, EntityInfo>();
        }

        public EntityInfo GetMapInfo<T>()
        {
            if( !_mapInfo.ContainsKey( typeof( T ).Name ) )
            {
                var entityMap = EntityInfo.BuildMap<T>( NextCounter() );
                entityMap.Builder = this;

                _mapInfo.Add( typeof( T ).Name, entityMap );
            }

            return _mapInfo[ typeof( T ).Name ];
        }

        private string ColumnsBuilder( EntityInfo entityInfo )
        {
            var columnBuilder = new StringBuilder();

            foreach( var item in entityInfo.Columns )
            {
                columnBuilder.Append( $"{entityInfo.Alias}.{item},");
            }

            columnBuilder.Remove( columnBuilder.Length - 1, 1 );

            return columnBuilder.ToString();
        }

        private string ParamsBuilder( EntityInfo entityInfo )
        {
            var paramBuilder = new StringBuilder();

            foreach( var item in entityInfo.Columns )
            {
                if( !entityInfo.PKeys.Contains( item ) )
                {
                    paramBuilder.AppendFormat( "@{0},", item.ToLower() );
                }
            }

            paramBuilder.Remove( paramBuilder.Length - 1, 1 );

            return paramBuilder.ToString();
        }

        public object GetValue<T>( string properyName, T obj )
        {
            return typeof( T ).GetProperty( properyName )
                              .GetValue( obj );
        }
    }
}
