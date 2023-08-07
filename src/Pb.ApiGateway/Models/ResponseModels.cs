using Newtonsoft.Json;
using System.Collections.Generic;

namespace Pb.ApiGateway.Models;

public class Geometry
{
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("coordinates")] public List<float> Coordinates { get; set; }
}

public class Feature
{
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("properties")] public Hotel Properties { get; set; }

    [JsonProperty("geometry")] public Geometry Geometry { get; set; }
}

public class GeoJsonResponse
{
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("features")] public IEnumerable<Feature> Features { get; set; }
}