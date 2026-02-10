using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryCore.interfaces;
using TelemetryCore.models;

namespace TelemetryCore.Builders
{
    /// <summary>
    /// директор для построения конфигураций анализаторов. реализует паттерн "строитель"
    /// </summary>
    public class AnalyzerDirector
    {

        public ITelemetryAnalyzer BuildMarsConfiguration(IAnalyzerBuilder builder)
        {
            builder.Reset();

            var calibration = new CalibrationData
            {
                ReferenceValue = 1.38,
                Offset = -15.0,
                CalibrationDate = DateTime.Now
            };
            builder.SetCalibration(calibration);

            var filters = new FilterConfig
            {
                MinThreshold = 5.0,
                MaxThreshold = 950.0,
                EnableNoiseReduction = true
            };
            builder.SetFilters(filters);

            builder.SetOutputFormat(OutputFormat.JSON);
            return builder.Build();
        }

        public ITelemetryAnalyzer BuildMoonConfiguration(IAnalyzerBuilder builder)
        {
            builder.Reset();

            var calibration = new CalibrationData
            {
                ReferenceValue = 1.16,
                Offset = -120.0,
                CalibrationDate = DateTime.Now
            };
            builder.SetCalibration(calibration);

            var filters = new FilterConfig
            {
                MinThreshold = 1.0,
                MaxThreshold = 999.0,
                EnableNoiseReduction = false
            };
            builder.SetFilters(filters);

            builder.SetOutputFormat(OutputFormat.XML);
            return builder.Build();
        }

        public ITelemetryAnalyzer BuildDefaultConfiguration(IAnalyzerBuilder builder)
        {
            builder.Reset();

            var calibration = new CalibrationData
            {
                ReferenceValue = 1.0,
                Offset = 0.0,
                CalibrationDate = DateTime.Now
            };
            builder.SetCalibration(calibration);

            var filters = new FilterConfig
            {
                MinThreshold = 0.0,
                MaxThreshold = 1000.0,
                EnableNoiseReduction = false
            };
            builder.SetFilters(filters);

            builder.SetOutputFormat(OutputFormat.PlainText);

            return builder.Build();

        }

        public ITelemetryAnalyzer BuildCustomConfiguration(IAnalyzerBuilder builder, CalibrationData calibration, FilterConfig filters, OutputFormat format)
        {
            builder.Reset();
            builder.SetCalibration(calibration);
            builder.SetFilters(filters);
            builder.SetOutputFormat(format);
            return builder.Build();
        }
    }
}
