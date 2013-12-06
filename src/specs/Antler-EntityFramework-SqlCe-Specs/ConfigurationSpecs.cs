// ReSharper disable InconsistentNaming
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.EntityFramework.SqlCe.Configuration;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.EntityFramework.Sqlite.Specs
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
                configurator.UseWindsorContainer().UseStorage(EntityFrameworkPlusSqlCe.Use);

                //assert                                
                configurator.Configuration.Container.Get<ISessionScopeFactory>().Should().BeOfType<EntityFrameworkSessionScopeFactory>(); 
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
                basicConfigurator.UseStorage(EntityFrameworkPlusSqlCe.Use);                                
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
                basicConfigurator.UseStorageNamed(EntityFrameworkPlusSqlCe.Use, "SuperStorage");                
            }
        }       
    }
}
// ReSharper restore InconsistentNaming
