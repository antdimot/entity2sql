using ADM.EntityToSQL.Meta;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADM.EntityToSQL.Tests.Entities
{
    [TableMap( Name = "USERS" )]
    public class User
    {
        [KeyMap( Name ="ID")]
        public int Id { get; set; }

        [ColumnMap( Name ="FIRST_NAME")]
        public string FirstName { get; set; }

        [ColumnMap( Name = "LAST_NAME" )]
        public string LastName { get; set; }
    }
}
