namespace Domain.Interfaces.JukeBox
{
    public interface ISoundTrackService
    {
        Task ConnectAndSubscribeAsync(string zoneId);
    }
}
