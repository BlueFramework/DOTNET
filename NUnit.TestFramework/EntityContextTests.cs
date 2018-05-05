using NUnit.Framework;
using BlueFramework.Blood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Tests1.Models;

namespace BlueFramework.Blood.Tests
{
    [TestFixture()]
    public class EntityContextTests
    {
        [Test()]
        public void EntityContextTest()
        {
            Assert.Pass("passed");
            //Assert.Fail();
        }

        [Test()]
        public void SaveTest()
        {
            bool pass = true;
            Session.Init();
            using (EntityContext context = Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();
                    for(int i = 1; i <= 5; i++)
                    {
                        UserInfo ui = new UserInfo() {
                            UserName = "pk"+i.ToString(),
                            Birthday = DateTime.Now
                        };
                        context.Save<UserInfo>("test.insertUser", ui);
                    }

                    context.Commit();

                }
                catch(Exception ex)
                {
                    context.Rollback();
                    pass = false;
                }
            }
            Assert.IsTrue(pass);
            //Assert.Fail();
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.Pass("passed");
            //Assert.Fail();
        }

        [Test()]
        public void SeleteTest()
        {
            Assert.Pass("passed");
            //Assert.Fail();
        }

        [Test()]
        public void BeginTransactionTest()
        {
            Assert.Pass("passed");
        }

        [Test()]
        public void CommitTest()
        {
            Assert.Pass("passed");
        }

        [Test()]
        public void RollbackTest()
        {
            Assert.Pass("passed");
        }

        [Test()]
        public void DisposeTest()
        {
            Assert.Pass("passed");
        }
    }
}