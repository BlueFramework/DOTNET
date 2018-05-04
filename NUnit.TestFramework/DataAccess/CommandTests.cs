using NUnit.Framework;
using BlueFramework.Blood.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueFramework.Blood.Config;
using NUnit.Tests1.Models;

namespace BlueFramework.Blood.DataAccess.Tests
{
    [TestFixture()]
    public class CommandTests
    {
        [Test()]
        public void SelectTest()
        {
            Command command = new Command();
            EntityConfig config = new EntityConfig();
            config.Id = "test.findUserById";
            config.Sql = "SELECT * FROM [USER] WHERE id=#{value}";
            UserInfo ui = command.Select<UserInfo>(config, 1);
            Assert.IsNotNull(ui);
        }

        [Test()]
        public void LoadEntityTest()
        {
            Assert.Fail();
        }
    }
}