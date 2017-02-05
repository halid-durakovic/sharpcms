using NUnit.Framework;
using sharpcms.database.config;

namespace sharpcms.database.tests.config
{
    [TestFixture]
    public class DbConnectionConfigServiceTests
    {
        private DbConnectionConfigService cfg;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            cfg = new DbConnectionConfigService();
        }

        [Test]
        public void Should_be_able_to_get_cfg()
        {
            var result = cfg.GetConnectionConfig("db1");

            Assert.That(result, Is.Not.Null);

            Assert.That(result.Name, Is.EqualTo("db1"));

            Assert.That(result.Provider, Is.Not.Null.Or.Empty);

            Assert.That(result.ConnectionString, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void Should_be_able_to_get_master_cfg()
        {
            var result = cfg.GetMasterConnectionConfig("db1");

            Assert.That(result, Is.Not.Null);

            Assert.That(result.Name, Is.EqualTo("db1"));

            Assert.That(result.Provider, Is.Not.Null.Or.Empty);

            Assert.That(result.ConnectionString, Contains.Substring("master"));
        }

        [Test]
        public void Should_throw_if_cfg_not_found()
        {
            Assert.Throws<DbConnectionConfigNotFoundException>(() =>
            {
                cfg.GetConnectionConfig("i dont exist");
            });
        }

        [Test]
        public void Should_be_able_to_get_cfg_db_name()
        {
            var result = cfg.GetConnectionConfigDbName("db1");

            Assert.That(result, Is.Not.Null.Or.Empty);

            Assert.That(result, Is.EqualTo("sharpcms.database.tests.db1"));
        }
    }
}