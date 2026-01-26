using NetCoreAI.Project03_RapidAPI.ViewModels;
using System.Net.Http.Headers;


var client = new HttpClient(); //yeni bir HttpClient nesnesi oluşturuluyor
List<ApiMoviesViewModel> apiMoviesViewModels = new List<ApiMoviesViewModel>(); // ApiMoviesViewModel türünde bir liste oluşturuluyor
var request = new HttpRequestMessage // yeni bir HttpRequestMessage nesnesi oluşturuluyor
{
    Method = HttpMethod.Get, // HTTP GET yöntemi kullanılıyor
    RequestUri = new Uri("https://imdb236.p.rapidapi.com/api/imdb/top250-movies"), // İstek yapılacak URL belirtiliyor
    Headers = //İstek başlıkları ekleniyor
    {
        { "x-rapidapi-key", "b869be713fmsh1f7976a7f113390p175f7djsnd28c78181ba6" }, // RapidAPI anahtarı
        { "x-rapidapi-host", "imdb236.p.rapidapi.com" },// RapidAPI host bilgisi
    },
};
using (var response = await client.SendAsync(request)) // İstek gönderiliyor ve yanıt alınıyor
{
    response.EnsureSuccessStatusCode(); // Yanıtın başarılı olup olmadığı kontrol ediliyor
    var body = await response.Content.ReadAsStringAsync(); // Yanıt içeriği okunuyor
    apiMoviesViewModels=System.Text.Json.JsonSerializer.Deserialize<List<ApiMoviesViewModel>>(body);// Yanıt içeriği(json) ApiMoviesViewModel türüne (string) deserialize ediliyor
    foreach (var movies in apiMoviesViewModels)
    {
        Console.WriteLine("-"+"Yapım Yılı:"+movies.releaseDate + " -"+"Tür:" +movies.interests[1] + " -"+"Film:" + movies.primaryTitle);
    }
}

Console.ReadLine();