using System.Xml; 
using System.Net.Http.Headers;
using System.Text;
using System.ServiceModel.Syndication; // RSS verilerini kolayca parçalayıp okumak için

class Program
{
    private static readonly string apiUrl = "https://router.huggingface.co/hf-inference/models/facebook/bart-large-cnn";
    private static readonly string hfToken = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== AI HABER AJANSI: SON 10 HABER ANALİZİ ===\n");

        string rssUrl = "https://www.bloomberght.com/rss";

        try
        {
            // RSS kaynağına bağlan ve içeriği oku
            using var reader = XmlReader.Create(rssUrl);
            var feed = SyndicationFeed.Load(reader);

            // Gelen haber listesinden ilk 10 tanesini seç
            var lastTenNews = feed.Items.Take(10).ToList();

            int counter = 1;
            foreach (var news in lastTenNews)
            {
                // Konsolu renklendirerek kullanıcıya bilgi ver
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[{counter}/10] İŞLENEN HABER: {news.Title.Text}");
                Console.ResetColor();

                // Haber başlığı ve özetini birleştir, içindeki HTML taglerini temizle
                string newsContent = news.Title.Text + ". " + (news.Summary?.Text ?? "");
                newsContent = System.Text.RegularExpressions.Regex.Replace(newsContent, "<.*?>", string.Empty);

                // Hazırlanan metni AI modeline gönder
                Console.WriteLine("[⏳] AI Özetliyor...");
                string summary = await SummarizeText(newsContent);

                // Sonucu ekrana bas
                Console.WriteLine("----------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("AI ÖZETİ: " + summary);
                Console.ResetColor();
                Console.WriteLine("----------------------------------------------------");

                counter++;
                // API sınırlarına takılmamak için her haber arası 1 saniye bekle
                await Task.Delay(1000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[❌] Hata: " + ex.Message);
        }

        Console.ReadKey();
    }

    // AI Modeliyle iletişimi sağlayan ana fonksiyon
    static async Task<string> SummarizeText(string input)
    {
        using var client = new HttpClient();
        // Hugging Face Token'ımızı isteğin başlığına ekliyoruz
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", hfToken);

        // API'ye gönderilecek JSON paketini hazırlıyoruz
        var payload = new
        {
            inputs = input, // Özetlenecek ham metin
            parameters = new { min_length = 20, max_length = 80 }, // Özetin uzunluk sınırları
            options = new { wait_for_model = true } // Model uykudaysa yüklenene kadar bekle
        };

        // Veriyi JSON formatına çevirip paketliyoruz
        var json = System.Text.Json.JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // API'ye POST isteği atıyoruz
        var response = await client.PostAsync(apiUrl, content);
        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            // Gelen JSON sonucundaki gereksiz tırnak ve parantezleri temizleyip sade metni dön
            return result.Trim('[', ']').Replace("{\"summary_text\":\"", "").Replace("\"}", "").Replace("\\\"", "\"");
        }
        else
        {
            return "Özet oluşturulamadı (Sunucu meşgul).";
        }
    }
}