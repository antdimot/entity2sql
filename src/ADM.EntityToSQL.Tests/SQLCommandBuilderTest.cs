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

            var selectCommand = sqlBuilder.MakeSelect<User>();

            Assert.IsTrue( selectCommand.Contains( "USERS" ) );
        }

        [TestMethod]
        public void MakeSelect_with_params_SHOULD_RETURN_SUCCESS()
        {
            var sqlBuilder = new SQLStatementBuilder();

            var selectCommand = sqlBuilder.MakeSelect<User>( o => o.FirstName == "Antonio" );

            Assert.IsTrue( selectCommand.Contains( "WHERE" ) );
        }
    }
}
