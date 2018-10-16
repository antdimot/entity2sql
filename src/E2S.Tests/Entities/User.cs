using E2S.Meta;
using System;
using System.Collections.Generic;
using System.Text;

namespace E2S.Tests.Entities
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
