using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#nullable disable

namespace BakuBus.Models
{
    public class Attributes
    {
        public string BUS_ID { get; set; }
        public string PLATE { get; set; }
        public string DRIVER_NAME { get; set; }
        public string PREV_STOP { get; set; }
        public string CURRENT_STOP { get; set; }
        public string SPEED { get; set; }
        public string BUS_MODEL { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public string ROUTE_NAME { get; set; }
        public int LAST_UPDATE_TIME { get; set; }
        public string DISPLAY_ROUTE_CODE { get; set; }

    }

    public class BUS
    {
        [JsonPropertyName("@attributes")]
        public Attributes attributes { get; set; }

        public override string ToString()
        {
            return attributes.DISPLAY_ROUTE_CODE;
        }
    }

    public class Bakubus
    {
        public List<BUS> BUS { get; set; }
        public List<string> Bus_Number { get; set; } = new();
        
    }
    

}
