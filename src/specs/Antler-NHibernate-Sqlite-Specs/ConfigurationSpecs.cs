// ReSharper disable InconsistentNaming

using Antler.NHibernate;
using Antler.NHibernate.Configuration;
using FluentAssertions;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.StructureMap;

namespace SmartElk.Antler.NHibernate.Sqlite.Specs
{
    public class ConfigurationSpecs
    {
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_configure_storage
        {
            [Test]
            public void should_set_storage()
            {                
                //arrange
                var configurator = new AntlerConfigurator();
                
                //act
                configurator.UseStructureMapContainer().UseStorage(NHibernateStorage.Use.WithDatabaseConfiguration(SQLiteConfiguration.Standard.InMemory()));

                //assert                                
                configurator.Configuration.Container.Get<ISessionScopeFactory>().Should().BeOfType<NHibernateSessionScopeFactory>(); 
            }
        }
        
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_configure_storage_without_container
        {
            [Test]
            [ExpectedException(typeof(ContainerRequiredException))]
            public void should_throw_exception()
            {
                //arrange
                var basicConfigurator = new AntlerConfigurator();

                //act
                basicConfigurator.UseStorage(NHibernateStorage.Use);                                
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_configure_named_storage_without_container
        {
            [Test]
            [ExpectedException(typeof(ContainerRequiredException))]
            public void should_throw_exception()
            {
                //arrange
                var basicConfigurator = new AntlerConfigurator();

                //act
                basicConfigurator.UseStorageNamed(NHibernateStorage.Use, "SuperStorage");                
            }
        }       
    }
}
// ReSharper restore InconsistentNaming
