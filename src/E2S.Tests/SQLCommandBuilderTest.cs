using E2S.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace E2S.Tests
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
