
namespace Mef.Autofac
{
    using System;

    public interface IExport<TContract> : IDisposable
    {
        TContract Instance { get; }
        dynamic Metadata { get; }
    }

}