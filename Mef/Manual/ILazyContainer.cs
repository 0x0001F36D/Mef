

namespace Mef.Manual
{
    using System;
    using System.Collections.Generic;
    using System.Composition;

    public interface ILazyContainer<TContract, TTranscription> : IDisposable, IEnumerable<ExportFactory<TContract, TTranscription>>
    {
    }

}
