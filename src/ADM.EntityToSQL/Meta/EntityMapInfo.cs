using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E2S.Meta
{
    public class EntityInfo
    {
        public string Table { get; set; } // Users

        public string[] Columns { get; set; } // Id, Firstname, Lastname

        public string[] Keys { get; set; } // Id

        public static EntityInfo BuildMap<T>()
        {
            var einfo = new EntityInfo();

            var tables = (TableMapAttribute[])typeof( T ).GetCustomAttributes( typeof( TableMapAttribute ), false );
            // check if exists the table map information for the type T
            if( tables.Count() != 1 ) throw new Exception( string.Format( "The type {0} contains {1}.", typeof( T ).Name, tables.Count() ) );

            // set table name
            einfo.Table = tables[0].Name;

            var columnArray = new ArrayList();
            var keyArray = new ArrayList();
            foreach( var property in typeof( T ).GetProperties() )
            {
                var columns = (ColumnMapAttribute[])property.GetCustomAttributes( typeof( ColumnMapAttribute ), true );
                // check if there are too much map information for the property
                if( columns.Count() > 1 ) throw new Exception(
                        string.Format( "The property {0} of type {0} contains more then one column map", property.Name, typeof( T ).Name ) );

                // check if the property has column map information
                if( columns.Count() > 0 )
                {
                    columnArray.Add( columns[0].Name );

                    if( columns[0] is KeyMapAttribute ) // check if is also a key
                    {
                        keyArray.Add( columns[0].Name );
                    }
                }
            }
            einfo.Columns = (string[])columnArray.ToArray( typeof( string ) );
            einfo.Keys = (string[])keyArray.ToArray( typeof( string ) );

            return einfo;
        }
    }
}
