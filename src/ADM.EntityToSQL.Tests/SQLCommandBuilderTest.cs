using ADM.EntityToSQL.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADM.EntityToSQL.Tests
{
    [TestClass]
    public class SQLCommandBuilderTest
    {
        [TestMethod]
        public void MakeSelectCommand_SHOULD_RETURN_SUCCESS()
        {
            var sqlBuilder = new SQLCommandBuilder();

            var selectCommand = sqlBuilder.MakeSelectCommand<User>();

            Assert.IsTrue( selectCommand.Contains( "USERS" ) );
        }
    }
}
