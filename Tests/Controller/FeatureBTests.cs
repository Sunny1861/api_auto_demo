using FluentAssertions;

namespace WebAPIDemo.Tests.Controller
{
    [TestClass]
    public class FeatureBTests
    {
        [TestMethod]
        public void FeatureBFuncA()
        {
            "a".Should().Be("a");
        }
    }
}