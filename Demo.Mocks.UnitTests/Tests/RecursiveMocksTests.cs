using NSubstitute;
using NUnit.Framework;

namespace Demo.Mocks.UnitTests.Tests
{
    [TestFixture]
    public class RecursiveMocksTests
    {
        [Test]
        public void given_one_mock_substitute_when_setting_properties_for_recursive_levels_of_depth_should_return_values_set_without_errors()
        {
            var context = Substitute.For<IContext>();
            context.CurrentRequest.Identity.Name.Returns("Todd Meinershagen");

            Assert.That(context.CurrentRequest.Identity.Name, Is.EqualTo("Todd Meinershagen"));
        }
    }

    public interface IContext
    {
        IRequest CurrentRequest { get; }
    }
    public interface IRequest
    {
        IIdentity Identity { get; }
        IIdentity NewIdentity(string name);
    }
    public interface IIdentity
    {
        string Name { get; }
        string[] Roles();
    }
}
