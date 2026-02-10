using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryCore.interfaces
{
    /// <summary>
    /// Абстрактная фабрика для создания анализаторов телеметрии. Реализует паттерн "Фабричный метод"
    /// </summary>
    public abstract class AnalyzerFactory
    {
        /// <summary>
        /// фабричный метод для создания конкретнргр анализатора
        /// </summary>
        /// <returns>экземпляр анализатора телеметрии</returns>
        public abstract ITelemetryAnalyzer CreateAnalyzer();

        /// <summary>
        /// получение имени плагина
        /// </summary>
        /// <returns>название плагина</returns>
        public abstract string GetPluginName();

        /// <summary>
        /// получение версии плагина
        /// </summary>
        /// <returns>версия плагина</returns>
        public abstract string GetPluginVersion();

        /// <summary>
        /// получение описания плагина
        /// </summary>
        /// <returns>краткое описание функциональности</returns>
        public virtual string GetPluginDescription()
        {
            return "Telemetry analyzer plugin";
        }

    }
}
