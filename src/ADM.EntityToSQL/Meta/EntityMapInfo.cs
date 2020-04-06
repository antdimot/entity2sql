using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADM.EntityToSQL.Meta
{
    public class EntityInfo
    {
        public string Table { get; set; } // Users

        public IDictionary<string,string> ColumnsDictionary { get; set; }

        public string[] Columns { get; set; } // Id, Firstname, Lastname

        public string[] Keys { get; set; } // Id

        public string GetColumnName( string propertyName )
        {
            return ColumnsDictionary[propertyName];
        }

        public static EntityInfo BuildMap<T>()
        {
            var einfo = new EntityInfo();

            var tables = (TableMapAttribute[])typeof( T )
                                .GetCustomAttributes( typeof( TableMapAttribute ), false );
            
            if( tables.Count() != 1 )
                throw new Exception( $"The type {typeof( T ).Name} contains {tables.Count()}." );

            einfo.Table = tables[0].Name;

            einfo.ColumnsDictionary = new Dictionary<string, string>();
            var keyArray = new ArrayList();

            foreach( var property in typeof( T ).GetProperties() )
            {
                var columns = (ColumnMapAttribute[])property
                                    .GetCustomAttributes( typeof( ColumnMapAttribute ), true );
                
                // check if there are too much map information for the property
                if( columns.Count() > 1 ) throw new Exception(
                        string.Format( "The property {0} of type {0} contains more then one column map", property.Name, typeof( T ).Name ) );

                // check if the property has column map information
                if( columns.Count() > 0 )
                {
                    einfo.ColumnsDictionary.Add( property.Name, columns[0].Name );

                    if( columns[0] is KeyMapAttribute ) // check if is a key
                    {
                        keyArray.Add( columns[0].Name );
                    }
                }
            }

            einfo.Columns = einfo.ColumnsDictionary.Select( item => item.Value ).ToArray();

            einfo.Keys = (string[])keyArray.ToArray( typeof( string ) );

            return einfo;
        }
    }
}
