using MongoDB.Driver.Builders;
using SmartElk.Antler.MongoDb.Configuration;
using SmartElk.Antler.Specs.Shared.Entities;
// ReSharper disable InconsistentNaming
using System.Linq;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Common.Container;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.NHibernate.Configuration;
using SmartElk.Antler.Specs.Shared.NHibernate.Mappings;

namespace SmartElk.Antler.Storages.Specs
{
    public class ConfigurationSpecs
    {
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_configure_to_use_multiple_storages_with_default_name
        {
            [Test]
            [ExpectedException(typeof (Assumes.InternalErrorException))]
            public void should_throw_exception()
            {
                //arrange
                var configurator = new AntlerConfigurator();

                //act
                configurator.UseBuiltInContainer()
                            .UseStorage(
                                NHibernateStorage.Use.WithDatabaseConfiguration(SQLiteConfiguration.Standard.InMemory())
                                                 .WithMappings(From.AssemblyWithType<CountryMap>().First()))
                            .
                             UseStorage(
                                 NHibernateStorage.Use.WithDatabaseConfiguration(SQLiteConfiguration.Standard.InMemory())
                                                  .WithMappings(From.AssemblyWithType<CountryMap>().First()));
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_configure_to_use_multiple_storages_with_same_name
        {
            [Test]
            [ExpectedException(typeof (Assumes.InternalErrorException))]
            public void should_throw_exception()
            {
                //arrange
                var configurator = new AntlerConfigurator();

                //act
                configurator.UseBuiltInContainer()
                            .UseStorage(
                                NHibernateStorage.Use.WithDatabaseConfiguration(SQLiteConfiguration.Standard.InMemory())
                                                 .WithMappings(From.AssemblyWithType<CountryMap>().First()), "Super")
                            .
                             UseStorage(
                                 NHibernateStorage.Use.WithDatabaseConfiguration(SQLiteConfiguration.Standard.InMemory())
                                                  .WithMappings(From.AssemblyWithType<CountryMap>().First()), "Super");
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_configure_mongodb_without_indexes
        {
             [Test]
            public void should_not_fail()
            {
                //act
                var configurator = new AntlerConfigurator();
                 configurator.UseBuiltInContainer()
                     .UseStorage(MongoDbStorage.Use("mongodb://localhost:27017", "AntlerTest")
                         .WithRecreatedDatabase(true));


            }
        }
    }
}

// ReSharper restore InconsistentNaming