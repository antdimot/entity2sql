using ADM.EntityToSQL.Builder;
using ADM.EntityToSQL.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADM.EntityToSQL.Tests
{
    [TestClass]
    public class SQLCommandBuilderTest
    {
        [TestMethod]
        public void MakeSelect_SHOULD_RETURN_SUCCESS()
        {
            var sqlBuilder = new SQLStatementBuilder();

            var selectCommand1 = sqlBuilder.MakeSelect<User>();

            Assert.IsTrue( selectCommand1.Contains( "USERS" ) );
        }

        [TestMethod]
        public void MakeSelect_with_params_SHOULD_RETURN_SUCCESS()
        {
            var sqlBuilder = new SQLStatementBuilder();

            var andSelect = sqlBuilder.MakeSelect<User>( o => o.FirstName == "Antonio" );

            var orSelect = sqlBuilder.MakeSelect<User>( o => o.FirstName == "Antonio" || o.LastName == "Di Motta" && o.Age == 45 );

            Assert.IsTrue( andSelect.Contains( "WHERE" ) );

            Assert.IsTrue( orSelect.Contains( "OR" ) );
        }

        [TestMethod]
        public void MakeJoin_with_params_SHOULD_RETURN_SUCCESS()
        {
            var sqlBuilder = new SQLStatementBuilder();

            var joinSelect = sqlBuilder.MakeJoin<User,Role>( (u,r) => u.Role.Id == r.Id );

            Assert.IsTrue( joinSelect.Contains( "JOIN" ) );
        }
    }
}
