using ADM.EntityToSQL.Meta;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADM.EntityToSQL.Tests.Entities
{
    [TableMap( Name = "ROLES" )]
    public class Role
    {
        [PKeyMap( Name = "ID" )]
        public int Id { get; set; }

        [ColumnMap( Name = "NAME" )]
        public string Name { get; set; }
    }
}
