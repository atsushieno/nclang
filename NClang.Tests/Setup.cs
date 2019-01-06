using NUnit.Framework;
using System;

namespace NClang.Tests
{
    // HACK for NUnit3:
    //   NUnit3 test adapter causes setting different default work directory both different adapters.
    //   It makes stable.
    //   https://github.com/nunit/nunit/issues/1072
    [SetUpFixture]
    public sealed class Setup
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }
    }
}
