using System.Data;
using NUnit.Framework;

namespace sharpcms.database.tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DbConnectionServiceTests
    {
        private DbConnectionService db;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            db = new DbConnectionService();
        }

        [Test]
        public void Should_be_able_to_create_and_connect_to_db_1()
        {
            db.CreateIfDoesNotExist("db1");

            using (var one = db.GetConnection("db1"))
            {
                Assert.That(one, Is.Not.Null); 

                Assert.That(one.Connection.State, Is.EqualTo(ConnectionState.Open)); 
            }

            db.DeleteIfDoesExist("db1");
        }

        [Test]
        public void Should_be_able_to_create_and_connect_to_db_2()
        {
            db.CreateIfDoesNotExist("db2");

            using (var one = db.GetConnection("db2"))
            {
                Assert.That(one, Is.Not.Null);

                Assert.That(one.Connection.State, Is.EqualTo(ConnectionState.Open));
            }

            db.DeleteIfDoesExist("db2");
        }
    }
}
