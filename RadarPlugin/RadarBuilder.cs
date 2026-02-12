using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryCore.interfaces;
using TelemetryCore.models;

namespace RadarPlugin
{
        public class RadarBuilder : IAnalyzerBuilder
        {
            private RadarAnalyzer _analyzer = new();

            public void SetCalibration(CalibrationData data)
            {
                _analyzer.Calibration = data;
            }

            public void SetFilters(FilterConfig filters)
            {
                _analyzer.Filters = filters;
            }

            public void SetOutputFormat(OutputFormat format)
            {
                _analyzer.OutputFormat = format;
            }

            public ITelemetryAnalyzer Build()
            {
                var result = _analyzer;
                Reset();
                return result;
            }

            public void Reset()
            {
                _analyzer = new RadarAnalyzer();
            }
        }
    
}
