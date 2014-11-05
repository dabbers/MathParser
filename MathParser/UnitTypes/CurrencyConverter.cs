using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Net;

namespace dab.Library.MathParser
{
    public enum CurrencyUnits
    {
        [UnitAbbreviation("$")]
        [UnitAbbreviation("United States Dollar")]
        [UnitPlural("United States Dollars")]
        [UnitType(UnitTypes.Currency)]
        USD,

        [UnitAbbreviation("$")]
        [UnitAbbreviation("Canadian Dollar")]
        [UnitPlural("Canadian Dollars")]
        [UnitType(UnitTypes.Currency)]
        CAD,

        [UnitAbbreviation("Euro")]
        [UnitAbbreviation("€")]
        [UnitPlural("Euros")]
        [UnitType(UnitTypes.Currency)]
        EUR,

        // Good grief britain
        [UnitAbbreviation("£")]
        [UnitAbbreviation("British Pounds")]
        [UnitAbbreviation("British Pound")]
        [UnitAbbreviation("Pound Sterlings")]
        [UnitAbbreviation("Pound Sterling")]
        [UnitAbbreviation("Sterling")]
        [UnitAbbreviation("Sterlings")]
        [UnitAbbreviation("Sterling Pound")]
        [UnitAbbreviation("Sterling Pounds")]
        [UnitPlural("British Pounds")]
        [UnitType(UnitTypes.Currency)]
        GBP,

        [UnitAbbreviation("Bitcoin")]
        [UnitAbbreviation("฿")]
        [UnitPlural("Bitcoins")]
        [UnitType(UnitTypes.Currency)]
        BTC,

        [UnitAbbreviation("Unknown")]
        Unknown
    }

    public class CurrencyConverter : UnitConverter
    {

        public override Enum BaseUnit { get { return CurrencyUnits.USD; } }
        
        public CurrencyConverter(string exchangeUrl, string cachePath)
        {
            this.exchangeUrl = exchangeUrl;
            this.cachePath = cachePath;
            this.exchangeRateData = new ExchangeRate() { Timestamp = 0 };
        }

        /// <summary>
        /// Converts from 1 distance to another. By default, converts to Meter and then back to the other measurements
        /// </summary>
        /// <param name="value">The value of the from unit</param>
        /// <param name="from">From unit can be anything listed in DistanceUnits</param>
        /// <param name="to">To unit can be anything listed in DistanceUnits</param>
        /// <returns></returns>
        public override decimal Convert(decimal value, Enum from, Enum to)
        {
            loadCacheFromDisk();

            if ((CurrencyUnits)from == CurrencyUnits.Unknown)
            {
                throw new InvalidUnitTypeException(from.ToString());
            }
            if ((CurrencyUnits)to == CurrencyUnits.Unknown)
            {
                throw new InvalidUnitTypeException(to.ToString());
            }

            value /= exchangeRateData.Rates[from.ToString()];

            return value *= exchangeRateData.Rates[to.ToString()];
        }

        private void loadCacheFromDisk()
        {
            if (!loadedCache && File.Exists(Path.Combine(this.cachePath, "currency_cache.json")))
            {
                using (StreamReader reader = new StreamReader(Path.Combine(this.cachePath, "currency_cache.json")))
                {
                    var json = reader.ReadToEnd();
                    if (!String.IsNullOrEmpty(json))
                    {
                        exchangeRateData = JsonConvert.DeserializeObject<ExchangeRate>(json);
                    }
                    
                }

                loadedCache = true;
            }



            var offset = DateTime.Now - new DateTime(1970, 1, 1).AddSeconds(exchangeRateData.Timestamp);
            if (offset.TotalHours > 16)
            {
                // Get new value
                updateCacheFromUrl();
            }
        }

        private void updateCacheFromUrl()
        {
            WebClient wc = new WebClient();
            var exchangeRates = wc.DownloadString(exchangeUrl);
            using (var file = File.CreateText(Path.Combine(this.cachePath, "currency_cache.json")))
            {
                file.Write(exchangeRates);
            }

            exchangeRateData = JsonConvert.DeserializeObject<ExchangeRate>(exchangeRates);


        }

        private string exchangeUrl = String.Empty;
        private bool loadedCache = false;
        private string cachePath = String.Empty;
        private ExchangeRate exchangeRateData;
    }

    public class ExchangeRate
    {
        [JsonProperty("disclaimer")]
        public string Disclaimer { get; set; }

        [JsonProperty("license")]
        public string Licence { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }


}
