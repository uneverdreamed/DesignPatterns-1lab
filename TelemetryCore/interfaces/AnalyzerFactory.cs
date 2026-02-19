using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryCore.interfaces
{
    public abstract class AnalyzerFactory
    {
        public abstract ITelemetryAnalyzer CreateAnalyzer();
        public abstract string GetPluginName();
        public abstract string GetPluginVersion();
        public virtual string GetPluginDescription()
        {
            return "Telemetry analyzer plugin";
        }

    }
}
