using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryCore.interfaces;

namespace SpectrometerPlugin
{
    public class SpectrometerFactory : AnalyzerFactory
    {
        public override ITelemetryAnalyzer CreateAnalyzer()
        {
            Console.WriteLine("[FACTORY] Creating Spectrometer Analyzer instance...");
            return new SpectrometerAnalyzer();
        }
   
        public override string GetPluginName()
        {
            return "Spectrometer Analyzer";
        }

        public override string GetPluginVersion()
        {
            return "1.0.0";
        }

        public override string GetPluginDescription()
        {
            return "Analyzes spectral data from spectrometer sensors. " +
                   "Processes wavelengths and intensities with calibration and filtering.";
        }
    }
}
    