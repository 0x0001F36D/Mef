
namespace Mef.Test.Models
{
    using Mef.Manual;
    using System.Collections.Generic;

    public sealed class TestTranscription : MetadataContract, ITestMetadata
    {
        public TestTranscription(IDictionary<string, object> properties) : base(properties)
        {
        }

        public string Description { get; private set; }
    }
}
