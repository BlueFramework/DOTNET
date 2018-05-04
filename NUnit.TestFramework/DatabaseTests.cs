using NUnit.Framework;
using BlueFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NUnit.Tests1
{
    [TestFixture()]
    public class DatabaseTests
    {
        [Test()]
        public void AddInParameterTest()
        {

        }

        [Test()]
        public void AddInParameterTest1()
        {

        }

        [Test()]
        public void AddInParameterTest2()
        {

        }

        [Test()]
        public void AddOutParameterTest()
        {

        }

        [Test()]
        public void AddParameterTest()
        {

        }

        [Test()]
        public void AddParameterTest1()
        {

        }

        [Test()]
        public void BuildParameterNameTest()
        {

        }

        [Test()]
        public void CreateConnectionTest()
        {

        }

        [Test()]
        public void ExecuteDataSetTest()
        {

        }

        [Test()]
        public void ExecuteDataSetTest1()
        {

        }

        [Test()]
        public void ExecuteDataSetTest2()
        {

        }

        [Test()]
        public void LoadDataSetTest()
        {
            
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            BlueFramework.Data.Database db = factory.CreateDefault();
            DataSet ds = db.ExecuteDataSet(CommandType.Text, "select * from SAUser");
            Assert.IsNotNull(ds);
        }

        [Test()]
        public void LoadDataSetTest1()
        {

        }

        [Test()]
        public void LoadDataSetTest2()
        {

        }

        [Test()]
        public void LoadDataSetTest3()
        {

        }

        [Test()]
        public void ExecuteNonQueryTest()
        {

        }

        [Test()]
        public void ExecuteNonQueryTest1()
        {

        }

        [Test()]
        public void ExecuteNonQueryTest2()
        {

        }

        [Test()]
        public void ExecuteNonQueryTest3()
        {

        }

        [Test()]
        public void ExecuteNonQueryTest4()
        {

        }

        [Test()]
        public void ExecuteNonQueryTest5()
        {

        }

        [Test()]
        public void ExecuteScalarTest()
        {

        }

        [Test()]
        public void ExecuteScalarTest1()
        {

        }

        [Test()]
        public void ExecuteScalarTest2()
        {

        }

        [Test()]
        public void ExecuteScalarTest3()
        {

        }

        [Test()]
        public void GetParameterValueTest()
        {

        }

        [Test()]
        public void GetSqlStringCommandTest()
        {

        }

        [Test()]
        public void GetStoredProcCommandTest()
        {

        }

        [Test()]
        public void GetStoredProcCommandTest1()
        {

        }
    }
}