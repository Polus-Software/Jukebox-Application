namespace Domain.Interfaces.JukeBox
{
    public interface IHttpClientService
    {
        Task<string> GetData(StringContent content);
    }
}
