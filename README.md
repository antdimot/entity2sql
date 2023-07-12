# Entity To SQL

A simple library for building SQL statements by entity class.

## Example

> Entity definitions:
```c#

    [TableMap( Name = "USERS" )]
    public class User
    {
        [PKeyMap( Name ="ID")]
        public int Id { get; set; }

        [ColumnMap( Name ="FIRST_NAME")]
        public string FirstName { get; set; }

        [ColumnMap( Name = "LAST_NAME" )]
        public string LastName { get; set; }

        [ColumnMap( Name = "AGE" )]
        public int Age { get; set; }

        [ColumnMap( Name = "RoleID" )]
        public Role Role { get; set; }
    }

    [TableMap( Name = "ROLES" )]
    public class Role
    {
        [PKeyMap( Name = "ID" )]
        public int Id { get; set; }

        [ColumnMap( Name = "NAME" )]
        public string Name { get; set; }
    }
```
> Usage:
```c#
    var sqlBuilder = new SQLStatementBuilder();

    var select = sqlBuilder.MakeSelect<User>( o => o.FirstName == "Antonio" || o.LastName == "Di Motta" && o.Age == 150 );

    // Result:
    // SELECT t1.ID,t1.FIRST_NAME,t1.LAST_NAME,t1.AGE,t1.RoleID FROM USERS t1
    // WHERE (t1.FIRST_NAME = 'Antonio' OR (t1.LAST_NAME = 'Di Motta' AND t1.AGE = 150 ))

    var join = sqlBuilder.MakeJoin<User,Role>( (u,r) => u.Role.Id == r.Id );

    // Result:
    // SELECT t1.ID,t1.FIRST_NAME,t1.LAST_NAME,t1.AGE,t1.RoleID,t2.ID,t2.NAME FROM USERS t1 INNER JOIN ROLES t2 ON t1.RoleID=t2.ID
```
