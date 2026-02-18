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
            Console.WriteLine("║   ЦЕНТР УПРАВЛЕНИЯ МИССИЕЙ - v1.0      ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            // === ЭТАП 1: Загрузка плагинов ===
            Console.WriteLine("= ЭТАП 1: Загрузка плагинов =");
            var loader = new PluginLoader();
            var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
            var factories = loader.LoadPlugins(pluginPath);

            if (factories.Count == 0)
            {
                Console.WriteLine("\n[ОШИБКА] Плагины не найдены! Скопируйте DLL файлы в папку 'plugins'.");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n[УСПЕХ] Загружено плагинов: {factories.Count}\n");

            // === ЭТАП 2: Демонстрация Фабричного метода ===
            Console.WriteLine("= ЭТАП 2: Демонстрация паттерна Фабричный метод =");

            foreach (var factory in factories)
            {
                Console.WriteLine($"\n= Тестирование {factory.GetPluginName()} =");

                // Создаём анализатор через фабрику
                var analyzer = factory.CreateAnalyzer();
                analyzer.Initialize();

                // Генерируем тестовые данные
                byte[] testData;
                if (factory.GetPluginName().Contains("спектрометра"))
                {
                    testData = TelemetrySimulator.GenerateSpectrometerData(30);
                }
                else if (factory.GetPluginName().Contains("радара"))
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
                Console.WriteLine("\n--- Отчёт (формат PlainText) ---");
                Console.WriteLine(analyzer.GetReport());

                analyzer.Terminate();
            }

            // === ЭТАП 3: Демонстрация Строителя ===
            Console.WriteLine("\n= ЭТАП 3: Демонстрация паттерна Строитель =");

            if (factories.Count > 0)
            {
                var factory = factories[0]; // Берём первый плагин
                Console.WriteLine($"\n--- Конфигурация {factory.GetPluginName()} через Строитель ---");

                // Создаём строитель (упрощённо - через рефлексию)
                IAnalyzerBuilder? builder = CreateBuilder(factory);

                if (builder != null)
                {
                    // Создаём директора
                    var director = new AnalyzerDirector();

                    // === Конфигурация 1: Mars ===
                    Console.WriteLine("\n--- Конфигурация: МАРСИАНСКАЯ МИССИЯ ---");
                    var marsAnalyzer = director.BuildMarsConfiguration(builder);
                    marsAnalyzer.Initialize();

                    byte[] marsData = factory.GetPluginName().Contains("спектрометра")
                        ? TelemetrySimulator.GenerateSpectrometerData(20)
                        : TelemetrySimulator.GenerateRadarData(15);

                    marsAnalyzer.ProcessData(marsData);
                    Console.WriteLine("\n--- Отчёт Марс (формат JSON) ---");
                    Console.WriteLine(marsAnalyzer.GetReport());
                    marsAnalyzer.Terminate();

                    // === Конфигурация 2: Moon ===
                    Console.WriteLine("\n--- Конфигурация: ЛУННАЯ МИССИЯ ---");
                    var MoonAnalyzer = director.BuildMoonConfiguration(builder);
                    MoonAnalyzer.Initialize();

                    byte[] MoonData = factory.GetPluginName().Contains("спектрометра")
                        ? TelemetrySimulator.GenerateSpectrometerData(20)
                        : TelemetrySimulator.GenerateRadarData(15);

                    MoonAnalyzer.ProcessData(MoonData);
                    Console.WriteLine("\n--- Отчёт Луна ---");
                    Console.WriteLine(MoonAnalyzer.GetReport());
                    MoonAnalyzer.Terminate();

                    // === Конфигурация 3: Custom ===
                    Console.WriteLine("\n--- Конфигурация: ПОЛЬЗОВАТЕЛЬСКАЯ ---");
                    builder.Reset();
                    builder.SetCalibration(new CalibrationData(2.0, 10.0));
                    builder.SetFilters(new FilterConfig(10.0, 500.0, true));
                    builder.SetOutputFormat(OutputFormat.PlainText);

                    var customAnalyzer = builder.Build();
                    customAnalyzer.Initialize();

                    byte[] customData = factory.GetPluginName().Contains("спектрометра")
                        ? TelemetrySimulator.GenerateSpectrometerData(20)
                        : TelemetrySimulator.GenerateRadarData(15);

                    customAnalyzer.ProcessData(customData);
                    Console.WriteLine("\n--- Отчёт пользовательской конфигурации ---");
                    Console.WriteLine(customAnalyzer.GetReport());
                    customAnalyzer.Terminate();
                }
            }

            // === ЗАВЕРШЕНИЕ ===
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║        ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА          ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        // Вспомогательный метод для создания строителя
        static IAnalyzerBuilder? CreateBuilder(AnalyzerFactory factory)
        {
            try
            {
                var assemblyName = factory.GetType().Assembly.GetName().Name;
                string builderTypeName;

                // Определяем имя строителя по имени сборки
                if (assemblyName == "RadarPlugin")
                {
                    builderTypeName = "RadarPlugin.RadarBuilder";
                }
                else if (assemblyName == "SpectrometerPlugin")
                {
                    builderTypeName = "SpectrometerPlugin.SpectrometerBuilder";
                }
                else
                {
                    Console.WriteLine($"[ВНИМАНИЕ] Неизвестный плагин: {assemblyName}");
                    return null;
                }

                var builderType = factory.GetType().Assembly.GetType(builderTypeName);

                if (builderType != null)
                {
                    return (IAnalyzerBuilder)Activator.CreateInstance(builderType)!;
                }
                else
                {
                    Console.WriteLine($"[ВНИМАНИЕ] Тип строителя не найден: {builderTypeName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ВНИМАНИЕ] Не удалось создать строитель: {ex.Message}");
            }

            return null;
        }
    }
}