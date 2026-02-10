using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryCore.interfaces
{
    /// <summary>
    /// Интерфейс анализатора телеметрии
    /// </summary>
    public interface ITelemetryAnalyzer
    {
        /// <summary>
        /// Инициализация анализатора
        /// </summary>
        void Initialize();
        /// <summary>
        /// Обработка сырых данных телеметрии
        /// </summary>
        /// <param name="rawData"></param>
        void ProcessData(byte[] rawData);
        /// <summary>
        /// Получение отчета о результатах анализа
        /// </summary>
        /// <returns></returns>
        string GetSupport();
        /// <summary>
        /// Завершение работы анализатора и освобождение ресурсов
        /// </summary>
        void Terminate();
    }
}
