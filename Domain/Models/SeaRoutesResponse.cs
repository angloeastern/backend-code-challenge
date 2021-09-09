using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Domain.Models
{
    public class Routepoint
    {

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("trackDistance")]
        public double TrackDistance { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("locode")]
        public string Locode { get; set; }

        [JsonProperty("clazz")]
        public int Clazz { get; set; }
    }

    public class GetRouteJson
    {
        [JsonProperty("journeytime")]
        public int Journeytime { get; set; }

        [JsonProperty("eta")]
        public int Eta { get; set; }

        [JsonProperty("routepoints")]
        public List<Routepoint> Routepoints { get; set; }

        [JsonProperty("via")]
        public List<object> Via { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("secaIntersection")]
        public double SecaIntersection { get; set; }

        [JsonProperty("piracyIntersection")]
        public double PiracyIntersection { get; set; }

        [JsonProperty("streetDistance")]
        public double StreetDistance { get; set; }
    }

    public class SeaRoutesResult
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("getRouteJson")]
        public List<GetRouteJson> GetRouteJson { get; set; }

        [JsonProperty("routingAreas")]
        public List<object> RoutingAreas { get; set; }
    }

}
