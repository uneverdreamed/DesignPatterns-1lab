using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryCore.models
{
    /// <summary>
    /// пакет телеметрических данных
    /// </summary>
    public class TelemetryPacket
    {
        /// <summary>
        /// временная метка получения данных
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// сырые данные от датчика
        /// </summary>
        public byte[] RawData { get; set; }

        /// <summary>
        /// тип сенсора от которого получены данные
        /// </summary>
        public string SensorType { get; set; }

        public TelemetryPacket(byte[] rawData, string sensorType)
        {
            Timestamp = DateTime.Now;
            RawData = rawData;
            SensorType = sensorType;
        }

    }
}
