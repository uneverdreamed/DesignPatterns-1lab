using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryCore.interfaces;
using TelemetryCore.models;

namespace RadarPlugin
{
    public class RadarFactory : AnalyzerFactory
    {
        public override ITelemetryAnalyzer CreateAnalyzer()
        {
            return new RadarAnalyzer();
        }

        public override string GetPluginName() => "Radar Analyzer";
        public override string GetPluginVersion() => "1.0.0";
    }
}
 