//Here we are returning the GraphQL queries to fetch or update(Using query for fetching, Mutations to make changes and subscription to get changes in the current playing song)
//details from soundtrack. These formats are defined by soundtrack API team. we need to use the exact format to connect with soundtrack.

using Newtonsoft.Json;

namespace Application.Common.JukeBox
{
    public class Query
    {
        public static string GetTokenQuery(string? username, string? password)
        {
            return "{\r\n  \"query\": \"mutation { loginUser(input: {email: \\\"" + username + "\\\", password: \\\"" + password + "\\\"}) { token refreshToken }}\"\r\n}";
        }

        public static string GetPlayListQuery(string soundZone)
        {
            return "{\r\n  \"query\": \"query MyQuery { soundZone( id: \\\"" + soundZone + "\\\" ) { name account { musicLibrary { playlists(first: 1) { edges { node { name id } } } } } } }\"\r\n}";
        }

        public static string GetPlayListSongsQuery(string playListId)
        {
            return "{\r\n  \"query\": \"query MyQuery { playlist(id: \\\"" + playListId + "\\\") { id name tracks(first: 100) { edges { node { title id album { title display { image { sizes { thumbnail } } } } artists { name } durationMs } } } }}\"\r\n}";
        }

        public static string GetNowPlayingQuery(string soundZone)
        {
            return "{\r\n  \"query\": \"query MyQuery { nowPlaying( soundZone: \\\"" + soundZone + "\\\" ) { soundZone track { title id album { title } artists { name } display { image { sizes { thumbnail } } } } }}\"\r\n}";
        }

        public static string GetCurrentVolumeQuery(string soundZone)
        {
            return "{\r\n  \"query\": \"query MyQuery { soundZone( id: \\\"" + soundZone + "\\\" ) { playback { volume } }}\"\r\n}";
        }

        public static string UpdateVolumeQuery(string soundZone, int volume)
        {
            return "{\r\n  \"query\": \"mutation MyMutation { setVolume(input: { soundZone: \\\"" + soundZone + "\\\", volume: " + volume + " }) { volume }}\"\r\n}";
        }

        public static string CreateQueueQuery(string soundZone, Array trackids, int actualQueueLength)
        {
            string tracklist = JsonConvert.SerializeObject(trackids);
            if (actualQueueLength > 1)
            {  //Queue will not play immediately because maybe another song in the queue already playing
                string data = "{\"query\":\"mutation MyMutation($tracks: [ID!]!) {\\n  soundZoneQueueTracks(\\n    input: {\\n      soundZone: \\\"" + soundZone + "\\\"\\n      tracks: $tracks\\n      immediate: false\\n      clearQueuedTracks: true\\n    }\\n  ){\\n    status\\n  }\\n}\",\"variables\":{\"tracks\":" + tracklist + "},\"operationName\":\"MyMutation\"}";
                return data;
            }
            else
            { // Queue will play immediately
                string data = "{\"query\":\"mutation MyMutation($tracks: [ID!]!) {\\n  soundZoneQueueTracks(\\n    input: {\\n      soundZone: \\\"" + soundZone + "\\\"\\n      tracks: $tracks\\n      immediate: true\\n      clearQueuedTracks: true\\n    }\\n  ){\\n    status\\n  }\\n}\",\"variables\":{\"tracks\":" + tracklist + "},\"operationName\":\"MyMutation\"}";
                return data;
            }
        }

        public static string GetSongsListQuery(Array trackids)
        {
            string tracklist = JsonConvert.SerializeObject(trackids);
            string data = "{\r\n    \"query\": \"query MyQuery($ids: [ID!]!) { tracks(ids: $ids) { title id artists { name } display { image { sizes { thumbnail } } } durationMs album { title } } }\",\r\n    \"variables\": { \"ids\": " + tracklist + " }}";
            return data;
        }

        public static string GetPlayBackHistory(string soundZone, int queueLength)
        {
            return "{\r\n  \"query\": \"query MyQuery { soundZone( id: \\\"" + soundZone + "\\\" ) { playbackHistory(last: " + queueLength + ") { edges { node { track { id } } } } }}\"\r\n}";
        }

        public static string SearchTrack(string searchContent)
        {
            return "{\r\n  \"query\": \"query MyQuery { search(type: track, query: \\\"" + searchContent + "\\\", first: 20) { edges { node { ... on Track { id title } } } }}\"\r\n}";
        }

        public static string GetNowPlayingSubscription(string soundZone)
        {
            return "subscription MySubscription {\r\n  nowPlayingUpdate(input: { soundZone: \"" + soundZone + "\" }) {\r\n    nowPlaying {\r\n      track {\r\n        album {\r\n          title\r\n        }\r\n        artists {\r\n          name\r\n        } display { image { sizes { thumbnail } } }\r\n        title\r\n        id\r\n      }\r\n    }\r\n  }\r\n},";

        }

        public static string GetPlaylistSubscription(string playlistId)
        {
            return "subscription MySubscription { playlistUpdate(input: { playlist: \"" + playlistId + "\" }) { playlist { tracks(first: 100) { edges { node { album { title } artists { name } id title display { image { sizes { thumbnail } } } durationMs } } } } }}";

        }
    }
}
