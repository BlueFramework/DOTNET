using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueFramework.User.Models;

namespace NUnit.Tests1
{
    public class DynamicSample
    {
        public string Name { get; set; }

        public int Add(int a, int b)
        {
            return a + b;
        }
    }

    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            UserInfo ui = new UserInfo();
            ui.UserId = 1;
            ui.UserName = "test";
            Assert.Pass("Your first passing test");

        }


        [Test]
        public void TestDynamic()
        {
            int i = 1;
            dynamic d1 = i;

            dynamic d2 = new DynamicSample();
            int a = d2.Add(1, 2);
        }
    }
}
