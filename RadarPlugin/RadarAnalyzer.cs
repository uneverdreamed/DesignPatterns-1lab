using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryCore.interfaces;
using TelemetryCore.models;

namespace RadarPlugin
{
    public class RadarAnalyzer : ITelemetryAnalyzer
    {
        public CalibrationData? Calibration { get; set; }
        public FilterConfig? Filters { get; set; }
        public OutputFormat OutputFormat { get; set; }

        private List<double> _distances = new();
        private List<double> _reflections = new();
        private bool _isInitialized;

        public RadarAnalyzer()
        {
            Calibration = new CalibrationData();
            Filters = new FilterConfig();
            OutputFormat = OutputFormat.PlainText;
        }

        public void Initialize()
        {
            Console.WriteLine("[РАДАР] Инициализирован");
            _distances.Clear();
            _reflections.Clear();
            _isInitialized = true;
        }

        public void ProcessData(byte[] rawData)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Анализатор не инициализирован");

            Console.WriteLine($"[РАДАР] Обработка {rawData.Length} байт...");

            for (int i = 0; i < rawData.Length - 1; i += 2)
            {
                double distance = (rawData[i] / 255.0) * 1000;

                double rawReflection = (rawData[i + 1] / 255.0) * 100;

                double reflection = (rawReflection * Calibration!.ReferenceValue) + Calibration.Offset;

                // ОТЛАДКА 
                if (i == 0) 
                {
                    Console.WriteLine($"[DEBUG] Сырое: {rawReflection:F2}, Калибровка: Ref={Calibration.ReferenceValue}, Offset={Calibration.Offset}, Результат: {reflection:F2}");
                    Console.WriteLine($"[DEBUG] Фильтры: Min={Filters!.MinThreshold}, Max={Filters.MaxThreshold}");
                }

                if (reflection >= Filters!.MinThreshold && reflection <= Filters.MaxThreshold)

                // ОТЛАДКА

                    if (reflection >= Filters!.MinThreshold && reflection <= Filters.MaxThreshold)
                {
                    _distances.Add(Math.Round(distance, 2));
                    _reflections.Add(Math.Round(reflection, 2));
                }
            }

            Console.WriteLine($"[РАДАР] Обнаружено {_distances.Count} эхо-сигналов");
        }

        public string GetReport()
        {
            if (_distances.Count == 0)
                return "Нет данных радара";

            return OutputFormat switch
            {
                OutputFormat.JSON => GenerateJson(),
                OutputFormat.PlainText => GenerateText(),
                _ => "Неизвестный формат"
            };
        }

        public void Terminate()
        {
            Console.WriteLine("[РАДАР] Завершён");
            _distances.Clear();
            _reflections.Clear();
            _isInitialized = false;
        }

        private string GenerateJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"эхо_сигналов\": {_distances.Count},");
            sb.AppendLine($"  \"ближайший_объект\": {_distances.Min():F2},");
            sb.AppendLine($"  \"сильнейшее_отражение\": {_reflections.Max():F2}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private string GenerateText()
        {
            var sb = new StringBuilder();
            sb.AppendLine("--- ОТЧЕТ РАДАРА ---");
            sb.AppendLine($"Обнаружено эхо-сигналов: {_distances.Count}");
            sb.AppendLine($"Ближайший объект: {_distances.Min():F2} m");
            sb.AppendLine($"Сильнейшее отражение: {_reflections.Max():F2}%");
            return sb.ToString();
        }
    }
}