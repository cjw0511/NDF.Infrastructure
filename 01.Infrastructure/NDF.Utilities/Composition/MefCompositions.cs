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
    public static class MefCompositions
    {
        private static CompositionContainer _container;

        internal static CompositionContainer Container
        {
            get
            {
                if (_container == null)
                {
                    AggregateCatalog catalog = new AggregateCatalog();
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
                        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        foreach (Assembly assembly in assemblies)
                        {
                            catalog.Catalogs.Add(new AssemblyCatalog(assembly));
                        }
                    }
                    _container = new CompositionContainer(catalog);
                }
                return _container;
            }
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
            Container.ComposeParts(attributedParts);
        }

    }
}
