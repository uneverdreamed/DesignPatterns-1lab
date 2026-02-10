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
    }
}
