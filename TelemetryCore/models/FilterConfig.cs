using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryCore.models
{
    /// <summary>
    /// конфигурация фильтров для обработки данных
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// минимальный порог значений (отсекаются меньшие)
        /// </summary>
        public double MinThreshold { get; set; }
        /// <summary>
        /// максимальный порог значений (отсекаются большие)
        /// </summary>
        public double MaxThreshold { get; set; }
        /// <summary>
        /// включить подавление шума
        /// </summary>
        public bool EnableNoiseReduction { get; set; }

        public FilterConfig() 
        {
            MinThreshold = 0.0;
            MaxThreshold = 1000.0;
            EnableNoiseReduction = false;
        }

        public FilterConfig(double minThreshold, double maxThreshold, bool enableNoiseReduction)
        {
            MinThreshold = minThreshold;
            MaxThreshold = maxThreshold;
            EnableNoiseReduction = enableNoiseReduction;
        }

    }
}
