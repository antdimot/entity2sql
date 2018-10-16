# Entity To SQL

A simple library for building SQL statements by entity class.

## Example

> Entity defintion:

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

> Usage:

    var sqlBuilder = new SQLCommandBuilder();

    // SELECT ID,FIRST_NAME,LAST_NAME FROM USERS
    var selectCommand = sqlBuilder.MakeSelectCommand<User>();