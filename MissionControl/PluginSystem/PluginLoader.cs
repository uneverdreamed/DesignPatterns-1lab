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
                Console.WriteLine($"[ЗАГРУЗЧИК] Папка плагинов не найдена: {pluginDirectory}");
                Directory.CreateDirectory(pluginDirectory);
                Console.WriteLine($"[ЗАГРУЗЧИК] Создана папка: {pluginDirectory}");
                return factories;
            }

            var dllFiles = Directory.GetFiles(pluginDirectory, "*.dll");
            Console.WriteLine($"[ЗАГРУЗЧИК] Найдено {dllFiles.Length} DLL файлов");

            foreach (var dllFile in dllFiles)
            {
                try
                {
                    Console.WriteLine($"[ЗАГРУЗЧИК] Загрузка {Path.GetFileName(dllFile)}...");
                    var assembly = Assembly.LoadFrom(dllFile);

                    var factoryTypes = assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AnalyzerFactory)));

                    foreach (var factoryType in factoryTypes)
                    {
                        var factory = (AnalyzerFactory)Activator.CreateInstance(factoryType)!;
                        factories.Add(factory);
                        Console.WriteLine($"[ЗАГРУЗЧИК] Загружен: {factory.GetPluginName()} v{factory.GetPluginVersion()}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ЗАГРУЗЧИК] Ошибка загрузки {Path.GetFileName(dllFile)}: {ex.Message}");
                }
            }

            return factories;
        }
    }
}