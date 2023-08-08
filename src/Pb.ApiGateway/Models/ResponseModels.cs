using Newtonsoft.Json;

namespace Pb.ApiGateway.Models;

public partial class Hotel
{
    [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
    public Feature[] Features { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }
}

public partial class Feature
{
    [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
    public Geometry Geometry { get; set; }

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
    public Properties Properties { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }
}

public partial class Geometry
{
    [JsonProperty("coordinates", NullValueHandling = NullValueHandling.Ignore)]
    public double[] Coordinates { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }
}

public partial class Properties
{
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("phone_number", NullValueHandling = NullValueHandling.Ignore)]
    public string PhoneNumber { get; set; }
}

public class GeoJsonResponse
{
    [JsonProperty("type")] public string? Type { get; set; }

    [JsonProperty("features")] public IEnumerable<Feature>? Features { get; set; }
}