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
            config.Sql = "SELECT ID USERID,USERNAME,BIRTHDAY FROM [USER] WHERE id=#{value}";
            UserInfo ui = command.Select<UserInfo>(config, 1);
            Assert.IsNotNull(ui);
        }

        [Test()]
        public void LoadEntityTest()
        {
            Assert.Pass();
        }

        [Test()]
        public void LoadEntitiesTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void CommandTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void LoadEntityTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void LoadEntitiesTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void SelectTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void SelectTest2()
        {
            Assert.Fail();
        }

        [Test()]
        public void InsertTest()
        {
            Assert.Fail();
        }
    }
}