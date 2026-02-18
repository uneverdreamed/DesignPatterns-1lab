using TelemetryCore.Builders;
using TelemetryCore.interfaces;
using TelemetryCore.models;
using MissionControl.PluginSystem;
using MissionControl.Simulation;
using TelemetryCore.interfaces;
using TelemetryCore.models;

namespace MissionControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║   MISSION CONTROL CENTER - v1.0        ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            // === ЭТАП 1: Загрузка плагинов ===
            Console.WriteLine("=== STAGE 1: Loading Plugins ===");
            var loader = new PluginLoader();
            var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
            var factories = loader.LoadPlugins(pluginPath);

            if (factories.Count == 0)
            {
                Console.WriteLine("\n[ERROR] No plugins found! Please copy plugin DLLs to the 'plugins' folder.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n[SUCCESS] Loaded {factories.Count} plugin(s)\n");

            // === ЭТАП 2: Демонстрация Фабричного метода ===
            Console.WriteLine("=== STAGE 2: Factory Method Pattern Demo ===");

            foreach (var factory in factories)
            {
                Console.WriteLine($"\n--- Testing {factory.GetPluginName()} ---");

                // Создаём анализатор через фабрику
                var analyzer = factory.CreateAnalyzer();
                analyzer.Initialize();

                // Генерируем тестовые данные
                byte[] testData;
                if (factory.GetPluginName().Contains("Spectrometer"))
                {
                    testData = TelemetrySimulator.GenerateSpectrometerData(30);
                }
                else if (factory.GetPluginName().Contains("Radar"))
                {
                    testData = TelemetrySimulator.GenerateRadarData(25);
                }
                else
                {
                    testData = new byte[50];
                }

                // Обрабатываем данные
                analyzer.ProcessData(testData);

                // Получаем отчёт
                Console.WriteLine("\n--- Report (PlainText format) ---");
                Console.WriteLine(analyzer.GetReport());

                analyzer.Terminate();
            }

            // === ЭТАП 3: Демонстрация Строителя ===
            Console.WriteLine("\n=== STAGE 3: Builder Pattern Demo ===");

            if (factories.Count > 0)
            {
                var factory = factories[0]; // Берём первый плагин
                Console.WriteLine($"\n--- Configuring {factory.GetPluginName()} with Builder ---");

                // Создаём строитель (упрощённо - через рефлексию)
                IAnalyzerBuilder? builder = CreateBuilder(factory);

                if (builder != null)
                {
                    // Создаём директора
                    var director = new AnalyzerDirector();

                    // === Конфигурация 1: Mars ===
                    Console.WriteLine("\n--- Configuration: MARS MISSION ---");
                    var marsAnalyzer = director.BuildMarsConfiguration(builder);
                    marsAnalyzer.Initialize();

                    byte[] marsData = factory.GetPluginName().Contains("Spectrometer")
                        ? TelemetrySimulator.GenerateSpectrometerData(20)
                        : TelemetrySimulator.GenerateRadarData(15);

                    marsAnalyzer.ProcessData(marsData);
                    Console.WriteLine("\n--- Mars Report (JSON format) ---");
                    Console.WriteLine(marsAnalyzer.GetReport());
                    marsAnalyzer.Terminate();

                    // === Конфигурация 2: Moon ===
                    Console.WriteLine("\n--- Configuration: LUNAR MISSION ---");
                    var lunarAnalyzer = director.BuildMoonConfiguration(builder);
                    lunarAnalyzer.Initialize();

                    byte[] lunarData = factory.GetPluginName().Contains("Spectrometer")
                        ? TelemetrySimulator.GenerateSpectrometerData(20)
                        : TelemetrySimulator.GenerateRadarData(15);

                    lunarAnalyzer.ProcessData(lunarData);
                    Console.WriteLine("\n--- Lunar Report ---");
                    Console.WriteLine(lunarAnalyzer.GetReport());
                    lunarAnalyzer.Terminate();

                    // === Конфигурация 3: Custom ===
                    Console.WriteLine("\n--- Configuration: CUSTOM ---");
                    builder.Reset();
                    builder.SetCalibration(new CalibrationData(2.0, 10.0));
                    builder.SetFilters(new FilterConfig(10.0, 500.0, true));
                    builder.SetOutputFormat(OutputFormat.PlainText);

                    var customAnalyzer = builder.Build();
                    customAnalyzer.Initialize();

                    byte[] customData = factory.GetPluginName().Contains("Spectrometer")
                        ? TelemetrySimulator.GenerateSpectrometerData(20)
                        : TelemetrySimulator.GenerateRadarData(15);

                    customAnalyzer.ProcessData(customData);
                    Console.WriteLine("\n--- Custom Report ---");
                    Console.WriteLine(customAnalyzer.GetReport());
                    customAnalyzer.Terminate();
                }
            }

            // === ЗАВЕРШЕНИЕ ===
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║        DEMONSTRATION COMPLETE          ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Вспомогательный метод для создания строителя
        static IAnalyzerBuilder? CreateBuilder(AnalyzerFactory factory)
        {
            try
            {
                var assemblyName = factory.GetType().Assembly.GetName().Name;
                var builderTypeName = $"{assemblyName}.{factory.GetPluginName().Replace(" Analyzer", "")}Builder";

                var builderType = factory.GetType().Assembly.GetType(builderTypeName);

                if (builderType != null)
                {
                    return (IAnalyzerBuilder)Activator.CreateInstance(builderType)!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Could not create builder: {ex.Message}");
            }

            return null;
        }
    }
}