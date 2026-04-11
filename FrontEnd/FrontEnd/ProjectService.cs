using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FrontEnd
{
    public class ProjectService
    {
        private readonly HttpClient _http;

        public ProjectService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<JsonElement>> GetProjects()
        {
            var result = await _http.GetFromJsonAsync<List<JsonElement>>("api/projects");
            return result ?? new List<JsonElement>();
        }
    }
}
