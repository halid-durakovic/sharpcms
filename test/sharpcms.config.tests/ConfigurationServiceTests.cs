using System.Linq;
using NUnit.Framework;

namespace sharpcms.config.tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ConfigurationServiceTests
    {
        private ConfigurationService cfg;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            cfg = new ConfigurationService();
        }

        [Test]
        public void Should_be_able_to_get_app_setting()
        {
            var result = cfg.Get("AnyAppSetting");

            Assert.That(result, Is.EqualTo("AnyAppSettingValue"));
        }

       
        [Test]
        public void Should_be_able_to_get_deserialised_basic_key_value_json_object()
        {
            var result = cfg.Get<AnyTypeSetting>();

            Assert.That(result.AnyTypeKey1, Is.EqualTo("AnyTypeKeyValue"));
        }

        [Test]
        public void Should_be_able_to_get_deserialised_nested_key_value_json_object()
        {
            var result = cfg.Get<AnyTypeSetting>();

            Assert.That(result.AnotherTypeSetting1, Is.Not.Null);

            Assert.That(result.AnotherTypeSetting1.AnotherTypeKey, Is.EqualTo("AnotherTypeKeyValue"));
        }

        [Test]
        public void Should_be_able_to_get_deserialised_nested_key_value_array_json_object()
        {
            var result = cfg.Get<AnyTypeSetting>();

            Assert.That(result.AnotherTypeSettingList1, Is.Not.Null);

            Assert.That(result.AnotherTypeSettingList1.Count, Is.Not.Zero);

            Assert.That(result.AnotherTypeSettingList1.First().AnotherTypeKey, Is.EqualTo("AnotherTypeKeyValueList"));
        }
    }
}
