using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryCore.interfaces;
using TelemetryCore.models;

namespace SpectrometerPlugin
{
    
    public class SpectrometerAnalyzer : ITelemetryAnalyzer
    {
        public CalibrationData? Calibration { get; set; }
        public FilterConfig? Filters { get; set; }
        public OutputFormat OutputFormat { get; set; }

        private List<double> _wavelengths = new();
        private List<double> _intensities = new();
        private bool _isInitialized;

        public SpectrometerAnalyzer()
        {
            Calibration = new CalibrationData();
            Filters = new FilterConfig();
            OutputFormat = OutputFormat.PlainText;
        }

        public void Initialize()
        {
            Console.WriteLine("[SPECTROMETER] Initialized");
            _wavelengths.Clear();
            _intensities.Clear();
            _isInitialized = true;
        }

        public void ProcessData(byte[] rawData)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Not initialized");

            Console.WriteLine($"[SPECTROMETER] Processing {rawData.Length} bytes...");
            
            for (int i = 0; i < rawData.Length - 1; i += 2)
            {
                double wavelength = 200 + (rawData[i] / 255.0) * 800;
                double rawIntensity = (rawData[i + 1] / 255.0) * 100;

                double intensity = (rawIntensity * Calibration!.ReferenceValue) + Calibration.Offset;

                if (intensity >= Filters!.MinThreshold && intensity <= Filters.MaxThreshold)
                {
                    _wavelengths.Add(Math.Round(wavelength, 2));
                    _intensities.Add(Math.Round(intensity, 2));
                }
            }

            Console.WriteLine($"[SPECTROMETER] Found {_wavelengths.Count} spectral lines");
        }

        public string GetReport()
        {
            if (_wavelengths.Count == 0)
                return "No data";

            return OutputFormat switch
            {
                OutputFormat.JSON => GenerateJson(),
                OutputFormat.PlainText => GenerateText(),
                _ => "Unknown format"
            };
        }
        public void Terminate()
        {
            Console.WriteLine("[SPECTROMETER] Terminated");
            _wavelengths.Clear();
            _intensities.Clear();
            _isInitialized = false;
        }

        private string GenerateJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"samples\": {_wavelengths.Count},");
            sb.AppendLine($"  \"peak_intensity\": {_intensities.Max():F2}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private string GenerateText()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== SPECTROMETER REPORT ===");
            sb.AppendLine($"Samples: {_wavelengths.Count}");
            sb.AppendLine($"Peak: {_intensities.Max():F2} at {_wavelengths[_intensities.IndexOf(_intensities.Max())]:F2} nm");
            return sb.ToString();
        }
    }
}
