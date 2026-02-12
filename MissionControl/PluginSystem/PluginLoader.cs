using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TelemetryCore.interfaces;

namespace MissionControl.PluginSystem
{
    public class PluginLoader
    {
        public List<AnalyzerFactory> LoadPlugins(string pluginDirectory)
        {
            var factories = new List<AnalyzerFactory>();

            if (!Directory.Exists(pluginDirectory))
            {
                Console.WriteLine($"[LOADER] Plugin directory not found: {pluginDirectory}");
                Directory.CreateDirectory(pluginDirectory);
                Console.WriteLine($"[LOADER] Created directory: {pluginDirectory}");
                return factories;
            }

            var dllFiles = Directory.GetFiles(pluginDirectory, "*.dll");
            Console.WriteLine($"[LOADER] Found {dllFiles.Length} DLL files");

            foreach (var dllFile in dllFiles)
            {
                try
                {
                    Console.WriteLine($"[LOADER] Loading {Path.GetFileName(dllFile)}...");
                    var assembly = Assembly.LoadFrom(dllFile);

                    var factoryTypes = assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AnalyzerFactory)));

                    foreach (var factoryType in factoryTypes)
                    {
                        var factory = (AnalyzerFactory)Activator.CreateInstance(factoryType)!;
                        factories.Add(factory);
                        Console.WriteLine($"[LOADER] Loaded: {factory.GetPluginName()} v{factory.GetPluginVersion()}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[LOADER] Error loading {Path.GetFileName(dllFile)}: {ex.Message}");
                }
            }

            return factories;
        }
    }
}