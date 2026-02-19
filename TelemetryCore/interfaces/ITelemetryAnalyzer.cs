using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryCore.interfaces
{

    public interface ITelemetryAnalyzer
    {

        void Initialize();
  
        void ProcessData(byte[] rawData);

        string GetReport();

        void Terminate();
    }
}
