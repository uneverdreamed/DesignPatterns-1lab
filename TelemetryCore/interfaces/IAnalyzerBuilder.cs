using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryCore.models;

namespace TelemetryCore.interfaces
{
    /// <summary>
    /// Интерфейс строителя анализатора телеметрии. Реализует паттерн "Строитель"
    /// </summary>
    public interface IAnalyzerBuilder
    {
        /// <summary>
        /// установить данные калибровки
        /// </summary>
        /// <param name="data"></param>
        void SetCalibration(CalibrationData data);

        /// <summary>
        /// установить конфигурацию фильтров
        /// </summary>
        /// <param name="filters"></param>
        void SetFilters(FilterConfig filters);

        /// <summary>
        /// установить формат вывода отчета
        /// </summary>
        /// <param name="format"></param>
        void SetOutputFormat(OutputFormat format);

        /// <summary>
        /// построить и вернуть настроенный анализатор
        /// </summary>
        /// <returns>готовый к работе анализатор</returns>
        ITelemetryAnalyzer Build();

        /// <summary>
        /// сбросить строитель для создания нового анализатора
        /// </summary>
        void Reset();
    }
}
