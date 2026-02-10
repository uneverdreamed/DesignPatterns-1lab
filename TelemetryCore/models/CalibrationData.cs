using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryCore.models
{
    /// <summary>
    /// данные калибровки для анализатора
    /// </summary>
    public class CalibrationData
    {
        /// <summary>
        /// эталонное значение для калибровки
        /// </summary>
        public double ReferenceValue { get; set; }
        /// <summary>
        /// смещение калибровки
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        /// дата последней калибровки
        /// </summary>
        public DateTime CalibrationDate { get; set; }

        public CalibrationData()
        {
            ReferenceValue = 1.0;
            Offset = 0.0;
            CalibrationDate = DateTime.Now;
        }

        public CalibrationData(double referenceValue, double offset)
        {
            ReferenceValue = referenceValue;
            Offset = offset;
            CalibrationDate = DateTime.Now;
        }
    }
}
