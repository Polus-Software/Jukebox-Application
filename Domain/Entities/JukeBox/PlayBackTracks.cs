using System.Text.Json.Serialization;

namespace Domain.Entities.JukeBox
{
    public class Songs
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class TrackId
    {
        [JsonPropertyName("Track")]
        public Songs Track { get; set; }
    }

    public class PlayBackTracks
    {
        [JsonPropertyName("node")]
        public TrackId Node { get; set; }
    }

}