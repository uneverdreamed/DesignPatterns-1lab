using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryCore.models
{
    public enum OutputFormat
    {
        /// <summary>
        /// формат JSON
        /// </summary>
        JSON,
        /// <summary>
        /// формат XML
        /// </summary>
        XML,
        /// <summary>
        /// простой тектовый формат
        /// </summary>
        PlainText
    }
}
