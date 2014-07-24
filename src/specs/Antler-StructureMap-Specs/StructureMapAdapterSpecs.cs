using System;
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Registration;
using SmartElk.Antler.Core.Common;

namespace SmartElk.Antler.StructureMap.Specs
{
    // ReSharper disable InconsistentNaming
    public class StructureMapAdapterSpecs
    {
        public class StructureMapScenario
        {
            protected IContainer Container;

            public StructureMapScenario()
            {
                Container = new StructureMapContainerAdapter();
            }
        }

		public interface IBoo : IDisposable
		{
			bool IsDisposed { get; }
		}

		public interface IBooEx
		{
		}

		public class Boo : IBoo, IBooEx
        {
            public bool IsDisposed { get; private set; }
            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        public class AnotherBoo : IBoo, IBooEx
        {
            public bool IsDisposed { get; private set; }
            public void Dispose()
            {
                IsDisposed = true;
            }
        }

		public class ChildBoo1 : Boo
		{
		}

		public class ChildBoo2 : Boo
		{
		}

		public class Xoo
		{
			public int SomeNumber { get; private set; }
			public string SomeString { get; set; }
			public IBoo SomeBoo { get; private set; }
			public IBoo AnyBoo { get; private set; }

			public Xoo(int someNumber, IBoo someBoo, IBoo anyBoo)
			{
				SomeNumber = someNumber;
				SomeBoo = someBoo;
				AnyBoo = anyBoo;
			}
		}

		[TestFixture]
		[Category("Unit")]
		public class when_registering_single_service_using_single_type : StructureMapScenario
		{
			[Test]
			public void should_be_available_from_container_by_interface()
			{
				Container.Put(Binding.Use<Boo>().As<IBoo>());

				Container.Has<IBoo>().Should().BeTrue();
				Container.Get<IBoo>().Should().BeOfType<Boo>();

				Container.Has(typeof(IBoo)).Should().BeTrue();
				Container.Get(typeof(IBoo)).Should().BeOfType<Boo>();
			}
		}

		[TestFixture]
		[Category("Unit")]
		public class when_resolving_single_object_using_factory_method : StructureMapScenario
		{
			[Test]
			public void should_be_available_from_container_by_factory_parameter_type()
			{
				Container.Put(Binding.Use(() => new AnotherBoo()).As<IBoo>());

				Container.Has<IBoo>().Should().BeTrue();
				Container.Get<IBoo>().Should().BeOfType<AnotherBoo>();

				Container.Has(typeof(IBoo)).Should().BeTrue();
				Container.Get(typeof(IBoo)).Should().BeOfType<AnotherBoo>();
			}
		}

		[TestFixture]
		[Category("Unit")]
		public class when_resolving_objects_registered_with_valid_name : StructureMapScenario
		{
			[Test]
			public void should_resolve_using_name()
			{
				Container.Put(Binding.Use<Boo>().As<IBoo>());
				Container.Put(Binding.Use<AnotherBoo>().As<IBoo>().Named("Special"));
				Container.Put(Binding.Use<ChildBoo1>().As<IBoo>());

				Container.Has<IBoo>().Should().BeTrue();
				Container.Has<IBoo>("Special").Should().BeTrue();
				//Container.Get<IBoo>().Should().BeOfType<Boo>(); // undefined actually
				Container.Get<IBoo>("Special").Should().BeOfType<AnotherBoo>();

				Container.Has(typeof(IBoo)).Should().BeTrue();
				//Container.Get(typeof(IBoo)).Should().BeOfType<Boo>(); // undefined actually
				Container.Get(typeof(IBoo), "Special").Should().BeOfType<AnotherBoo>();
			}
		}

		[TestFixture]
		[Category("Unit")]
		public class when_resolving_objects_using_invalid_name : StructureMapScenario
		{
			[Test]
			public void should_throw()
			{
				Container.Put(Binding.Use<AnotherBoo>().As<IBoo>().Named("Special"));

				Container.Has<IBoo>("Ololo").Should().BeFalse();

				Container.Invoking(c => c.Get<IBoo>("Ololo")).ShouldThrow<BindingNotRegisteredException>();
				Container.Invoking(c => c.Get(typeof(IBoo), "Ololo")).ShouldThrow<BindingNotRegisteredException>();
			}
		}

        [TestFixture]
        [Category("Unit")]
        public class when_resolving_multiple_objects_using_transient_lifestyle : StructureMapScenario
        {
            [Test]
            public void should_use_multiple_instances()
            {
                Container.Put(Binding.Use<Boo>().As<IBoo>().With(Lifestyle.Transient));

                var boo1 = Container.Get<IBoo>();
                var boo2 = Container.Get<IBoo>();

                boo1.Should().NotBe(boo2);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_resolving_multiple_objects_using_singleton_lifestyle : StructureMapScenario
        {
            [Test]
            public void should_use_one_instance()
            {
                Container.Put(Binding.Use<Boo>().As<IBoo>().With(Lifestyle.Singleton));

                var boo1 = Container.Get<IBoo>();
                var boo2 = Container.Get<IBoo>();

                boo1.Should().Be(boo2);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_releasing_object_with_transient_lifestyle : StructureMapScenario
        {
            [Test]
            [Ignore("Not implemented")]
            public void should_release()
            {
				Container.Put(Binding.Use<Boo>().As<IBoo>().With(Lifestyle.Transient));

                var boo = Container.Get<IBoo>();

                Container.Release(boo);

                boo.IsDisposed.Should().BeTrue();
            }
        }

		[TestFixture]
		[Category("Unit")]
		public class when_releasing_object_with_unmanaged_lifestyle : StructureMapScenario
		{
			[Test]
			[Ignore("Not implemented")]
			public void should_no_nothing()
			{
				Container.Put(Binding.Use<Boo>().As<IBoo>().With(Lifestyle.Unmanaged));

				var boo = Container.Get<IBoo>();

				Container.Release(boo);

				boo.IsDisposed.Should().BeFalse();
			}
		}

		[TestFixture]
		[Category("Unit")]
		public class when_resolving_object_registered_as_is : StructureMapScenario
		{
			[Test]
			public void should_resolve_by_type()
			{
				Container.Put(Binding.Use<Boo>());

				var boo = Container.Get<Boo>();
				boo.Should().NotBeNull();
			}
		}

		[TestFixture]
		[Category("Unit")]
		public class when_resolving_multiple_implementations_registered_separately : StructureMapScenario
		{
			[Test]
			public void should_resolve_in_registration_order()
			{
				var instances = new IBoo[] { new AnotherBoo(), new ChildBoo2(), new Boo(), new ChildBoo1() };

				instances.ForEach(i => Container.Put(Binding.Use(i).As<IBoo>()));

				var resolved = Container.GetAll<IBoo>();

				resolved.Should().ContainInOrder(instances);
			}
		}

		[TestFixture]
        [Category("Unit")]
        public class when_registering_multiple_types_as_is : StructureMapScenario
        {
            [Test]
            public void each_type_should_be_available_from_container_by_its_type()
            {
                Container.Put(Binding.Use(Core.Common.Reflection.AllTypes.BasedOn<IBoo>()));

                Container.Has<IBoo>().Should().BeFalse();
                Container.Has<Boo>().Should().BeTrue();
                Container.Has<AnotherBoo>().Should().BeTrue();
                Container.Get<AnotherBoo>().Should().BeOfType<AnotherBoo>();
            }
        }

		[TestFixture]
		[Category("Unit")]
		public class when_registering_multiple_types_with_forward_type : StructureMapScenario
		{
			[Test]
			public void each_type_should_be_available_from_container_by_forward_type()
			{
                Container.Put(Binding.Use(Core.Common.Reflection.AllTypes.BasedOn<IBoo>()).As<IBoo>());

				Container.Has<IBoo>().Should().BeTrue();
				Container.Has<Boo>().Should().BeFalse();
				Container.Has<AnotherBoo>().Should().BeFalse();
				//Container.Get<IBoo>().Should().BeOfType<Boo>();

				Container.GetAll<IBoo>().Should().HaveCount(4);
				Container.GetAll(typeof(IBoo)).Should().HaveCount(4);
			}
		}

		[TestFixture]
		[Category("Unit")]
		public class when_registering_multiple_types_with_multiple_forward_types : StructureMapScenario
		{
			[Test]
			public void each_type_should_be_available_from_container_by_any_forward_type()
			{
                Container.Put(Binding.Use(Core.Common.Reflection.AllTypes.BasedOn<IBoo>()).As(typeof(IBoo), typeof(IBooEx)));

				Container.Has<IBoo>().Should().BeTrue();
				Container.Has<Boo>().Should().BeFalse();
				Container.Has<AnotherBoo>().Should().BeFalse();
				//Container.Get<IBoo>().Should().BeOfType<Boo>();

				Container.GetAll<IBoo>().Should().HaveCount(4);
				Container.GetAll(typeof(IBoo)).Should().HaveCount(4);
				Container.GetAll<IBooEx>().Should().HaveCount(4);
				Container.GetAll(typeof(IBooEx)).Should().HaveCount(4);
			}
		}

		[TestFixture]
        [Category("Unit")]
        public class when_resolving_single_non_registered_component : StructureMapScenario
        {
            [Test]
            public void should_throw()
            {
				Container.Invoking(c => c.Get<Boo>()).ShouldThrow<BindingNotRegisteredException>();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_resolving_all_components_for_non_registered_service : StructureMapScenario
        {
            [Test]
            public void should_return_empty_list()
            {
                Container.GetAll<Boo>().Should().HaveCount(0);
            }
        }

		[TestFixture]
		[Category("Unit")]
		public class when_resolving_object_with_dependencies_from_app_settings : StructureMapScenario
		{
			[Test]
			[Ignore("Not implemented")]
			public void should_resolve_from_app_settings()
			{
				var boo = new Boo();

				Container.Put(Binding.Use<AnotherBoo>());
				Container.Put(Binding.Use<Boo>().As<IBoo>());
				Container.Put(Binding.Use(boo).As<IBoo>().Named("SpecialBoo"));
				Container.Put(Binding.Use<Xoo>());

				var xoo = Container.Get<Xoo>();

				xoo.Should().NotBeNull();
				xoo.SomeNumber.Should().Be(666);
				xoo.SomeString.Should().Be("Hail Satan!");
				xoo.SomeBoo.Should().Be(boo);
				xoo.AnyBoo.Should().BeOfType<AnotherBoo>();
			}
		}

        [TestFixture]
        [Category("Unit")]        
        public class when_accessing_native_container : StructureMapScenario
        {
            [Test]
            public void should_provide_the_actual_container()
            {
                Container.Put(Binding.Use<Boo>().As<IBoo>());

                Container.OnNative(c => c.Model.HasImplementationsFor<IBoo>().Should().BeTrue());
            }
        }


    }
    // ReSharper restore InconsistentNaming

}
