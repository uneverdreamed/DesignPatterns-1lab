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
            Console.WriteLine("[RADAR] Initialized");
            _distances.Clear();
            _reflections.Clear();
            _isInitialized = true;
        }

        public void ProcessData(byte[] rawData)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Not initialized");

            Console.WriteLine($"[RADAR] Processing {rawData.Length} bytes...");

            for (int i = 0; i < rawData.Length - 1; i += 2)
            {
                double distance = (rawData[i] / 255.0) * 1000;

                double rawReflection = (rawData[i + 1] / 255.0) * 100;

                double reflection = (rawReflection * Calibration!.ReferenceValue) + Calibration.Offset;

                if (reflection >= Filters!.MinThreshold && reflection <= Filters.MaxThreshold)
                {
                    _distances.Add(Math.Round(distance, 2));
                    _reflections.Add(Math.Round(reflection, 2));
                }
            }

            Console.WriteLine($"[RADAR] Detected {_distances.Count} echoes");
        }

        public string GetReport()
        {
            if (_distances.Count == 0)
                return "No radar data";

            return OutputFormat switch
            {
                OutputFormat.JSON => GenerateJson(),
                OutputFormat.PlainText => GenerateText(),
                _ => "Unknown format"
            };
        }

        public void Terminate()
        {
            Console.WriteLine("[RADAR] Terminated");
            _distances.Clear();
            _reflections.Clear();
            _isInitialized = false;
        }

        private string GenerateJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"echoes\": {_distances.Count},");
            sb.AppendLine($"  \"nearest_object\": {_distances.Min():F2},");
            sb.AppendLine($"  \"strongest_reflection\": {_reflections.Max():F2}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private string GenerateText()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== RADAR REPORT ===");
            sb.AppendLine($"Echoes detected: {_distances.Count}");
            sb.AppendLine($"Nearest object: {_distances.Min():F2} m");
            sb.AppendLine($"Strongest reflection: {_reflections.Max():F2}%");
            return sb.ToString();
        }
    }
}