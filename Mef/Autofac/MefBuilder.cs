
namespace Mef.Autofac
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Composition;
    using System.Composition.Convention;
    using System.Composition.Hosting;
    using System.Dynamic;
    using System.Reflection;

    public static class MefBuilder
    {
        public static ILazyContainer<TContract> Import<TContract>(this IEnumerable<Assembly> assemblies)
            where TContract : class
        {
            return new Importer<TContract>(assemblies);
        }

        public sealed class Importer<TContract> : IDisposable, IReadOnlyList<IExport<TContract>>, ILazyContainer<TContract>
            where TContract : class
        {


            private readonly CompositionHost _container;
            private readonly ReadOnlyCollection<IExport<TContract>> exportes;

            internal Importer(IEnumerable<Assembly> assemblies)
            {
                var target = typeof(TContract);

                if (!target.IsInterface)
                    throw new NotSupportedException($"type <{target}> should be an interface");


                var conventionBuilder = new ConventionBuilder();
                conventionBuilder.ForTypesMatching(x => !x.IsAbstract && !x.IsInterface && x.IsClass && x.GetInterface(target.FullName) == target).Export<TContract>();

                this._container = new ContainerConfiguration()
                      .WithAssemblies(assemblies, conventionBuilder)
                      .CreateContainer();

                var list = new List<IExport<TContract>>();

                var exported = this._container.GetExports<ExportFactory<TContract, IDictionary<string, object>>>();

                foreach (var e in exported)
                {
                    var expando = new ExpandoObject() as IDictionary<string, object>;
                    foreach (var exp in e.Metadata)
                    {
                        expando.Add(exp);
                    }
                    var lz = e.CreateExport();
                    list.Add(new Proxy(lz, expando));
                }
                this.exportes = new ReadOnlyCollection<IExport<TContract>>(list);
            }



            private class Proxy : IDisposable, IExport<TContract>
            {
                private readonly Export<TContract> _export;

                internal Proxy(Export<TContract> export, IDictionary<string, object> expando)
                {
                    this._export = export;
                    this.Metadata = expando;
                }

                public dynamic Metadata { get; }

                public TContract Instance => this._export.Value;

                public void Dispose() => this._export.Dispose();
            }

            public IExport<TContract> this[int index] => this.exportes[index];

            public int Count => this.exportes.Count;

            public IEnumerator<IExport<TContract>> GetEnumerator() => this.exportes.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => this.exportes.GetEnumerator();
            public void Dispose() => this._container.Dispose();
        }
    }

}