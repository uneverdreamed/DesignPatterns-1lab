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
            Console.WriteLine("[ФАБРИКА] Создание экземпляра анализатора спектрометра...");
            return new SpectrometerAnalyzer();
        }
   
        public override string GetPluginName()
        {
            return "Анализатор спектрометра";
        }

        public override string GetPluginVersion()
        {
            return "1.0.0";
        }

        public override string GetPluginDescription()
        {
            return "Анализирует спектральные данные от датчиков спектрометра. " +
                   "Обрабатывает длины волн и интенсивности с калибровкой и фильтрацией.";
        }
    }
}
    