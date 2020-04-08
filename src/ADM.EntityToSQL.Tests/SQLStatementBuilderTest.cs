using ADM.EntityToSQL.Builder;
using ADM.EntityToSQL.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADM.EntityToSQL.Tests
{
    [TestClass]
    public class SQLStatementBuilderTest
    {
        [TestMethod]
        public void MakeSelect_SHOULD_RETURN_SUCCESS()
        {
            var check = "SELECT t1.ID,t1.FIRST_NAME,t1.LAST_NAME,t1.AGE,t1.RoleID FROM USERS t1";

            var sqlBuilder = new SQLStatementBuilder();

            var result = sqlBuilder.MakeSelect<User>();

            Assert.IsTrue( check.Equals(result) );
        }

        [TestMethod]
        public void MakeSelect_with_params_SHOULD_RETURN_SUCCESS()
        {
            var check1 = "SELECT t1.ID,t1.FIRST_NAME,t1.LAST_NAME,t1.AGE,t1.RoleID FROM USERS t1 WHERE t1.FIRST_NAME = 'Antonio'";
            var check2 = "SELECT t1.ID,t1.FIRST_NAME,t1.LAST_NAME,t1.AGE,t1.RoleID FROM USERS t1 WHERE (t1.FIRST_NAME = 'Antonio' OR (t1.LAST_NAME = 'Di Motta' AND t1.AGE = 45))";

            var sqlBuilder = new SQLStatementBuilder();

            var result1 = sqlBuilder.MakeSelect<User>( o => o.FirstName == "Antonio" );

            Assert.IsTrue( check1.Equals( result1 ) );

            var result2 = sqlBuilder.MakeSelect<User>( o => o.FirstName == "Antonio" || o.LastName == "Di Motta" && o.Age == 45 );

            Assert.IsTrue( check2.Equals( result2 ) );
        }

        [TestMethod]
        public void MakeJoin_SHOULD_RETURN_SUCCESS()
        {
            var check = "SELECT t1.ID,t1.FIRST_NAME,t1.LAST_NAME,t1.AGE,t1.RoleID,t2.ID,t2.NAME FROM USERS t1 INNER JOIN ROLES t2 ON t1.RoleID=t2.t2.ID";

            var sqlBuilder = new SQLStatementBuilder();

            var result = sqlBuilder.MakeJoin<User,Role>( (u,r) => u.Role.Id == r.Id );

            Assert.IsTrue( check.Equals(result) );
        }

        [TestMethod]
        public void MakeInsert_SHOULD_RETURN_SUCCESS()
        {
            var check = "INSERT INTO USERS (ID,FIRST_NAME,LAST_NAME,AGE,RoleID) VALUES (@id,@first_name,@last_name,@age,@roleid)";

            var sqlBuilder = new SQLStatementBuilder();

            var result = sqlBuilder.MakeInsert<User>();

            Assert.IsTrue( check.Equals( result ) );
        }

        [TestMethod]
        public void MakeUpdate_SHOULD_RETURN_SUCCESS()
        {
            var check = "UPDATE USERS SET FIRST_NAME=@first_name,LAST_NAME=@last_name,AGE=@age,RoleID=@roleid WHERE ID = 1";

            var sqlBuilder = new SQLStatementBuilder();

            var result = sqlBuilder.MakeUpdate<User>( u => u.Id == 1 ) ;

            Assert.IsTrue( check.Equals( result ) );
        }
    }
}
