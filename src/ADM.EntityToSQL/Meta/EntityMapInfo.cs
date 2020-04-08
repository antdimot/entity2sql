using ADM.EntityToSQL.Builder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADM.EntityToSQL.Meta
{
    public class EntityInfo
    {
        public SQLStatementBuilder Builder { get; set; }

        public int Id { get; private set; }

        public string Table { get; set; }

        public IDictionary<string,string> ColumnsDictionary { get; private set; }

        public string[] Columns { get; set; } // Id, Firstname, Lastname

        public string[] PKeys { get; set; }

        public string Alias { get; private set; } // table alias

        public object TypeAssociated { get; private set; }

        public string GetColumnName( string propertyName )
        {
            return ColumnsDictionary[propertyName];
        }

        public EntityInfo( Type associated, int entityId )
        {
            Id = entityId;

            Alias = $"t{Id}";

            TypeAssociated = associated;

            ColumnsDictionary = new Dictionary<string, string>();
        }

        public static EntityInfo BuildMap<T>( int entityId )
        {
            var einfo = new EntityInfo( typeof(T), entityId );

            var tables = (TableMapAttribute[])typeof( T )
                                .GetCustomAttributes( typeof( TableMapAttribute ), false );
            
            if( tables.Count() != 1 )
                throw new Exception( $"The type {typeof( T ).Name} contains {tables.Count()} table definition." );

            einfo.Table = tables[0].Name;

            var keyArray = new ArrayList();
            foreach( var property in typeof( T ).GetProperties() )
            {
                var columns = (ColumnMapAttribute[])property
                                    .GetCustomAttributes( typeof( ColumnMapAttribute ), true );
                
                // check if there are too much map information for the property
                if( columns.Count() > 1 ) throw new Exception( $"The property {property.Name} of type {typeof( T ).Name } contains more then one column map." );

                // check if the property has column map information
                if( columns.Count() > 0 )
                {
                    einfo.ColumnsDictionary.Add( $"{property.Name}", columns[0].Name );

                    if( columns[0] is PKeyMapAttribute ) // if primary key
                    {
                        keyArray.Add( columns[0].Name );
                    }
                }
            }

            einfo.Columns = einfo.ColumnsDictionary.Select( item => item.Value ).ToArray();

            einfo.PKeys = (string[])keyArray.ToArray( typeof( string ) );

            return einfo;
        }
    }
}
