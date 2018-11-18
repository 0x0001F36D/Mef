
namespace Mef.Autofac
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ILazyContainer<TContract> : IDisposable, IEnumerable<IExport<TContract>>
        where TContract : class
    {
    }

}