using System.Net.Http.Json;

namespace Tests.Common
{
  
    public static class TestExtensions
    {
        public static async Task<T?> ToResponseModel<T>(this HttpResponseMessage response)
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
