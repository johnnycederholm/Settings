using System.Collections.Generic;
using Should;
using Xunit;
using System;
using Settings.Tests.Models;

namespace Settings.Tests
{
    public class SettingDeserializerTests
    {
        private readonly Dictionary<string, string> settings;
        private readonly SettingDeserializer deserializer;

        public SettingDeserializerTests()
        {
            settings = new Dictionary<string, string>();
            deserializer = new SettingDeserializer();
        }

        [Fact]
        public void ShouldThrowExceptionWhenPassingNullDictionary()
        {
            Assert.Throws<ArgumentNullException>(() => deserializer.Deserialize<SettingModel>(null));
        }

        [Fact]
        public void ShouldReturnInstansiatedModelIfDictionaryIsEmpty()
        {
            SettingModel actual = deserializer.Deserialize<SettingModel>(new Dictionary<string, string>());

            actual.ShouldNotBeNull();
        }

        [Fact]
        public void ShouldSetStringPropertyValueWhenFindingMatchingPropertyInModel()
        {
            string expected = "SomeValue";
            settings.Add("SomeString", "SomeValue");

            SettingModel actual = deserializer.Deserialize<SettingModel>(settings);

            actual.SomeString.ShouldEqual(expected);
        }

        [Fact]
        public void ShouldSetIntegerPropertyValyeWhenFindingMatchingPropertyInModel()
        {
            int expected = 1000;
            settings.Add("SomeInteger", "1000");

            SettingModel actual = deserializer.Deserialize<SettingModel>(settings);

            actual.SomeInteger.ShouldEqual(expected);
        }

        [Fact]
        public void ShouldSetBooleanPropertyValueWhenFindingMatchingPropertyInModel()
        {
            settings.Add("SomeBoolean", "true");

            SettingModel actual = deserializer.Deserialize<SettingModel>(settings);

            actual.SomeBoolean.ShouldBeTrue();
        }

        [Fact]
        public void ShouldSetCharPropertyValueWhenFindingMatchingPropertyInModel()
        {
            char expected = 'Y';
            settings.Add("SomeChar", "Y");

            SettingModel actual = deserializer.Deserialize<SettingModel>(settings);

            actual.SomeChar.ShouldEqual(expected);
        }

        [Fact]
        public void ShouldSetDateTimePropertyValueWhenFindingMatchingPropertyInModel()
        {
            DateTime expected = new DateTime(2016, 4, 22);
            settings.Add("SomeDate", "2016-04-22");

            SettingModel actual = deserializer.Deserialize<SettingModel>(settings);

            actual.SomeDate.ShouldEqual(expected);
        }

        [Fact]
        public void ShouldSetDecimalPropertyValueWhenFindingMatchingPropertyInModel()
        {
            decimal expected = 3.14M;
            settings.Add("SomeDecimal", "3,14");

            SettingModel actual = deserializer.Deserialize<SettingModel>(settings);

            actual.SomeDecimal.ShouldEqual(expected);
        }

        [Fact]
        public void ShouldSetDoublePropertyValueWhenFindingMatchingPropertyInModel()
        {
            double expected = 3.14;
            settings.Add("SomeDouble", "3,14");

            SettingModel actual = deserializer.Deserialize<SettingModel>(settings);

            actual.SomeDouble.ShouldEqual(expected);
        }

        [Fact]
        public void ShouldSetPropertyValueWhenFindingMatchingPropertyOnSecondLevel()
        {
            string expected = "Storgatan 10";
            settings.Add("Address.Street", expected);

            Person actual = deserializer.Deserialize<Person>(settings);

            actual.Address.ShouldNotBeNull();
            actual.Address.Street.ShouldEqual(expected);
        }

        [Fact]
        public void ShouldSetPropertyValueWhenFindingMatchingPropertyOnThirdLevel()
        {
            int expected = 46;
            settings.Add("Address.Country.CountryCode", expected.ToString());

            Person actual = deserializer.Deserialize<Person>(settings);

            actual.Address.Country.ShouldNotBeNull();
            actual.Address.Country.CountryCode.ShouldEqual(expected);
        }

        [Fact]
        public void ShouldNotThrowExceptionIfMatchingPropertyCantBeFound()
        {
            settings.Add("Address.Countr.CountryCode", "");

            Person actual = deserializer.Deserialize<Person>(settings);

            actual.Address.ShouldNotBeNull();
            actual.Address.Country.ShouldBeNull();
        }
    }
}
