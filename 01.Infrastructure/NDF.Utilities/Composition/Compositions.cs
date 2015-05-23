using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Composition
{
    /// <summary>
    /// 提供一组用于基于微软 MEF 框架快速解析依赖注入对象的工具 API。
    /// </summary>
    public static class Compositions
    {
        private static Lazy<CompositionContainer> _container = new Lazy<CompositionContainer>(CreateContainer);
        private static object _locker = new object();


        /// <summary>
        /// 获取当前用于管理 MEF 部件的 <see cref="CompositionContainer"/> 容器对象。
        /// </summary>
        internal static CompositionContainer Container
        {
            get { return _container.Value; }
        }


        /// <summary>
        /// 从特性化对象的数组创建可组合部件，并在指定的组合容器中组合这些部件。
        /// </summary>
        /// <param name="attributedParts"></param>
        public static void ComposeParts(IEnumerable<object> attributedParts)
        {
            ComposeParts(attributedParts.ToArray());
        }

        /// <summary>
        /// 从特性化对象的数组创建可组合部件，并在指定的组合容器中组合这些部件。
        /// </summary>
        /// <param name="attributedParts"></param>
        public static void ComposeParts(params object[] attributedParts)
        {
            lock (_locker)
            {
                Container.ComposeParts(attributedParts);
            }
        }


        private static CompositionContainer CreateContainer()
        {
            AggregateCatalog catalog = new AggregateCatalog();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(assembly));
            }

            try
            {
                string path = AppDomain.CurrentDomain.SetupInformation != null ? AppDomain.CurrentDomain.SetupInformation.PrivateBinPath : null;
                if (string.IsNullOrWhiteSpace(path))
                    path = AppDomain.CurrentDomain.BaseDirectory;

                DirectoryCatalog directory = new DirectoryCatalog(path);
                directory.Changing += (sender, arg) =>
                {
                    ((DirectoryCatalog)sender).Refresh();
                };
                catalog.Catalogs.Add(directory);
            }
            catch
            {
            }

            return new CompositionContainer(catalog);
        }

    }
}
