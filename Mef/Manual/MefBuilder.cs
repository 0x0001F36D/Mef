

namespace Mef.Manual
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Composition;
    using System.Composition.Convention;
    using System.Composition.Hosting;
    using System.Reflection;

    public static class MefBuilder
    {


        public static ILazyContainer<TContract, TTranscription> Composite<TAbstractContract, TContract, TMetadataContract, TTranscription>(this IEnumerable<Assembly> assemblies)
              where TAbstractContract : class, TContract
              where TContract : class
              where TTranscription : MetadataContract, TMetadataContract
              => new MefContainer<TAbstractContract, TContract, TMetadataContract, TTranscription>(assemblies);

        public static ILazyContainer<TContract, TTranscription> Composite<TContract, TMetadataContract, TTranscription>(this IEnumerable<Assembly> assemblies)
            where TContract : class
            where TTranscription : MetadataContract, TMetadataContract
            => new MefContainer<TContract, TContract, TMetadataContract, TTranscription>(assemblies);


        private sealed class MefContainer<TAbstractContract, TContract, TMetadataContract, TTranscription>
            : IEnumerable<ExportFactory<TContract, TTranscription>>, IDisposable, ILazyContainer<TContract, TTranscription>

            where TAbstractContract : class, TContract
            where TContract : class
            where TTranscription : MetadataContract, TMetadataContract
        {
            private readonly CompositionHost _container;
            private readonly IEnumerable<ExportFactory<TContract, TTranscription>> _exported;

            internal MefContainer(IEnumerable<Assembly> assemblies)
            {
                var conventionBuilder = new ConventionBuilder();
                conventionBuilder.ForTypesDerivedFrom<TAbstractContract>().Export<TContract>();

                this._container = new ContainerConfiguration()
                      .WithAssemblies(assemblies, conventionBuilder)
                      .CreateContainer();
                this._exported = this._container.GetExports<ExportFactory<TContract, TTranscription>>();
            }

            void IDisposable.Dispose() => this._container.Dispose();
            public IEnumerator<ExportFactory<TContract, TTranscription>> GetEnumerator() => this._exported.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => this._exported.GetEnumerator();
        }
    }
}
