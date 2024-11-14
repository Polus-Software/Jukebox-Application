namespace Domain.Entities.JukeBox
{
    public class TrackListResponse
    {
        public List<Track> Tracks { get; set; }
    }

    public class Track
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string AlbumTitle { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<string> Artists { get; set; }
        public int durationMs { get; set; }
        public int Count { get; set; }
        public string LastplayedSongId { get; set; }
    }

    // Temporary classes to match the JSON structure
    public class TempResponse
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public TempPlaylist Playlist { get; set; }
    }

    public class TempPlaylist
    {
        public string Name { get; set; }
        public TempTrackList Tracks { get; set; }
    }

    public class TempTrackList
    {
        public int Total { get; set; }
        public List<Edge> Edges { get; set; }
    }

    public class Edge
    {
        public TempTrack Node { get; set; }
    }

    public class TempTrack
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public Album Album { get; set; }
        public List<Artist> Artists { get; set; }
        public int durationMs { get; set; }
    }

    public class Album
    {
        public string Title { get; set; }
        public Display Display { get; set; }
    }

    public class Display
    {
        public Image Image { get; set; }
    }

    public class Image
    {
        public Sizes Sizes { get; set; }
    }

    public class Sizes
    {
        public string Thumbnail { get; set; }
    }

    public class Artist
    {
        public string Name { get; set; }
    }
}
