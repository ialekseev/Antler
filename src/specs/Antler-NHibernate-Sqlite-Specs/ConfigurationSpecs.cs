// ReSharper disable InconsistentNaming
using Antler.NHibernate;
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Abstractions;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.NHibernate.Sqlite.Configuration;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.NHibernate.Specs
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
                configurator.UseWindsorContainer().UseStorage(NHibernatePlusSqlite.Use);

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
                basicConfigurator.UseStorage(NHibernatePlusSqlite.Use);                                
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
                basicConfigurator.UseStorageNamed(NHibernatePlusSqlite.Use, "SuperStorage");                
            }
        }       
    }
}
// ReSharper restore InconsistentNaming
