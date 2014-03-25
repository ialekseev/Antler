// ReSharper disable InconsistentNaming

using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Core.Common;

namespace SmartElk.Antler.Common.Specs
{
    public class OptionSpecs
    {
        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_option_from_reference_null
        {
            [Test]
            public void should_return_none_option()
            {
                //arrange
                Option<string> option = (string)null;

                //assert
                option.IsSome.Should().BeFalse();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_option_from_nullable_null
        {
            [Test]
            public void should_return_none_option()
            {
                //arrange
                Option<int?> option = (int?)null;

                //assert
                option.IsSome.Should().BeFalse();
            }
        }                   
    }
}

// ReSharper restore InconsistentNaming