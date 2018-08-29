using System;
using System.IO;
using System.Reflection;
using System.Collections;

namespace Alaric.Utils
{
    /// <summary>
    /// A simple manager provides Plugin Manage service.
    /// </summary>
    /// <typeparam name="T">Plugin interface or abstract class.</typeparam>
    public class PluginManager<T>
    {
        private readonly ArrayList _loadedPlugins = new ArrayList();
        private readonly string _pluginFolder;

        /// <summary>
        /// Initialize a PluginManager.
        /// </summary>
        public PluginManager(string pluginFolder)
        {
            if (!Directory.Exists(pluginFolder))
                Directory.CreateDirectory(pluginFolder);

            _pluginFolder = pluginFolder;
        }

        /// <summary>
        /// Load plugins.
        /// </summary>
        public void LoadPlugs()
        {
            foreach (var file in Directory.GetFiles(_pluginFolder))
            {
                if (!file.ToUpper().EndsWith(".DLL"))
                    continue;
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    Type[] types = assembly.GetTypes();
                    foreach (Type t in types)
                        if (t.GetInterface(typeof(T).Name) != null)
                            _loadedPlugins.Add((T) assembly.CreateInstance(t.FullName ?? throw new InvalidOperationException()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Returns a ArrayList contains loaded plugins' instances.
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList GetLoadedPlugins() => _loadedPlugins;
    }
}