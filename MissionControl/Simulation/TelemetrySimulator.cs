using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControl.Simulation
{
    public class TelemetrySimulator
    {
        private static Random _random = new Random();

        public static byte[] GenerateSpectrometerData(int samples = 50)
        {
            Console.WriteLine($"[СИМУЛЯТОР] Генерация {samples} образцов спектрометра...");
            byte[] data = new byte[samples * 2];

            for (int i = 0; i < samples * 2; i += 2)
            {
                // случайная длина волны
                data[i] = (byte)_random.Next(0, 256);

                // случайная интенсивность
                data[i + 1] = (byte)_random.Next(20, 200);
            }

            return data;
        }

        public static byte[] GenerateRadarData(int echoes = 40)
        {
            Console.WriteLine($"[СИМУЛЯТОР] Генерация {echoes} эхо-сигналов радара...");
            byte[] data = new byte[echoes * 2];

            for (int i = 0; i < echoes * 2; i += 2)
            {
                // дистанция (от близкого к дальнему)
                data[i] = (byte)_random.Next(10, 256);

                // интенсивность отражения
                data[i + 1] = (byte)_random.Next(30, 180);
            }

            return data;
        }
    }
}
