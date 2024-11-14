using System.Text;

namespace Application.Common
{
  public static class AuthHelper
  {
    public static string GetBasicAuthToken(string username, string password)
    {
      var credentials = $"{username}:{password}";
      var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
      return $"Basic {base64Credentials}";
    }
  }
}
