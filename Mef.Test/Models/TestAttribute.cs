
namespace Mef.Test.Models
{
    using System;
    using System.Composition;
    using System.Text;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TestAttribute : ExportAttribute, ITestMetadata
    {
        public TestAttribute(string description) : base(typeof(ITestContract))
        {
            this.Description = description;
        }
        public string Description { get; }
    }
}
