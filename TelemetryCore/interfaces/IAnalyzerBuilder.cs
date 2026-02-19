using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryCore.models;

namespace TelemetryCore.interfaces
{
    public interface IAnalyzerBuilder
    {
        void SetCalibration(CalibrationData data);
        void SetFilters(FilterConfig filters);
        void SetOutputFormat(OutputFormat format);
        ITelemetryAnalyzer Build();
        void Reset();
    }
}
