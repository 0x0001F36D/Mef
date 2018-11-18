
namespace Mef.Test
{
    using Mef.Manual;
    using Mef.Autofac;
    using Mef.Test.Models;
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Linq;
    using System.Reflection;
    using Xunit;
    using System.Diagnostics;

    public class MefTest
    {
        private readonly Assembly[] _asms = AppDomain.CurrentDomain.GetAssemblies(); 

        [Fact]
        public void Autofac()
        {
            var container = _asms.Import<ITestContract>();

            var f = container.First();
            Assert.True(f is IExport<ITestContract>);
            Assert.Equal(f.Metadata.Description, DESC);
        }

        [Fact]
        public void Manual()
        {

            var container = _asms.Composite<ITestContract, ITestMetadata, TestTranscription>();

            var f = container.First();
            Assert.True(f is IExport<ITestContract>);
            Assert.Equal(f.Metadata.Description, DESC);
        }

        public const string DESC = "Desc";
    }


    [Test(MefTest.DESC)]
    public sealed class TestAddon : ITestContract
    {
    }





}
