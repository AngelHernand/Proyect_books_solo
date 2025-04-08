using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class OpenLibraryService
{
    private readonly HttpClient _httpClient;

    public OpenLibraryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Book>> SearchBooksAsync(string query)
    {
        var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OpenLibraryResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Docs.Select(d => new Book
            {
                Title = d.title,
                Author = d.author_name?.FirstOrDefault(),
                CoverId = d.cover_i
            }).ToList() ?? new List<Book>();
        }

        return new List<Book>();
    }
}

public class OpenLibraryResponse
{
    public List<OpenLibraryDoc> Docs { get; set; }
}

public class OpenLibraryDoc
{
    public string title { get; set; }
    public List<string> author_name { get; set; }
    public int? cover_i { get; set; }
}

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int? CoverId { get; set; }

    public string CoverUrl => CoverId.HasValue
        ? $"https://covers.openlibrary.org/b/id/{CoverId}-L.jpg"
        : "https://via.placeholder.com/150";
}
