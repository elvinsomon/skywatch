// PERMITE QUE LOS DATOS SE GUARDEN EN FetchData //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlertWebFinal.Data;

namespace AlertWebFinal.Service
{
    public class WeatherForecastService
    {
       public WeatherForecast MetricsDataInstance { get; } = new WeatherForecast();

     public List<WeatherForecast> MetricsDataList { get; } = new List<WeatherForecast>();

        public void AgregarConjunto(WeatherForecast conjunto)
        {
            var nuevoConjunto = new WeatherForecast
            {
                cpuName = conjunto.cpuName,
                MetricValue = conjunto.MetricValue,
                Hostname = conjunto.Hostname,
                Date = conjunto.Date
            };

            MetricsDataList.Add(nuevoConjunto);
        }

    }
}
