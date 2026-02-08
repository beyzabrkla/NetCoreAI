using Newtonsoft.Json; 
using System.Text;   
using HtmlAgilityPack; 

class Program
{
    private static readonly string apikey = "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";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== AI Web Veri Kazıyıcı ===");

        Console.Write("\nLütfen analiz edilecek URL'yi giriniz: ");
        string url = Console.ReadLine();

        if (!string.IsNullOrEmpty(url))
        {
            try
            {
                // Web sitesinin kaynak kodlarınıindiriyoruz
                Console.WriteLine($"\n[1/3] {url} adresine bağlanılıyor...");
                string htmlContent = await GetWebPageContent(url);

                // İndirilen karmaşık HTML içinden sadece işe yarar metinleri ayıklıyoruz
                Console.WriteLine("[2/3] Veriler ayıklanıyor ve AI'ya gönderiliyor...");
                string cleanText = ExtractTextFromHtml(htmlContent);

                // Ayıklanan temiz metni yapay zekaya gönderip analiz ettiriyoruz
                string aiResponse = await AnalyzeWebData(cleanText);

                // Yapay zekadan gelen özet raporu ekrana yazdırıyoruz
                Console.WriteLine("\n[3/3] SİTEDEN ELDE EDİLEN ANALİZ:");
                Console.WriteLine("===========================================");
                Console.WriteLine(aiResponse);
                Console.WriteLine("===========================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nBir hata oluştu: " + ex.Message);
            }
        }

        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }

    // Bu metot, belirtilen URL'ye gider ve sayfanın tüm HTML kodunu bir string olarak döner
    static async Task<string> GetWebPageContent(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            // Sitenin bizi "bot" olarak hemen reddetmemesi için tarayıcı taklidi yapıyoruz
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            return await client.GetStringAsync(url);
        }
    }

    // Bu metot HTML kodu içinden sadece Başlık (h1,h2,h3) ve Paragraf (p) yazılarını seçer
    static string ExtractTextFromHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html); // Ham HTML kodunu kütüphaneye yüklüyoruz

        // XPath kullanarak sadece metin içeren etiketleri seçiyoruz
        var nodes = doc.DocumentNode.SelectNodes("//h1 | //h2 | //h3 | //p");

        if (nodes == null) return "Sayfada okunabilir metin içeriği bulunamadı.";

        // Seçilen etiketlerin içindeki yazıları birleştiriyoruz
        string allText = string.Join(" ", nodes.Select(n => n.InnerText.Trim()));

        // Yapay zeka modelinin kapasitesini aşmamak için metni 4000 karakterle sınırlıyoruz
        return allText.Length > 4000 ? allText.Substring(0, 4000) : allText;
    }

    // Temizlenmiş metni Groq API'sine gönderiyor ve bir özet/analiz istiyor
    static async Task<string> AnalyzeWebData(string text)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apikey}");

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile", 
                messages = new[]
                {
                    new {
                        role = "system",
                        content = "Sen bir web veri analiz uzmanısın. Sana iletilen karmaşık web içeriğinden en önemli bilgileri, başlıkları veya ürün bilgilerini ayıkla ve düzgün bir liste halinde Türkçe olarak özetle."
                    },
                    new { role = "user", content = $"Şu web içeriğini analiz et: {text}" }
                }
            };

            // Veriyi JSON formatına dönüştürüp API'ye POST isteği atıyoruz
            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            string responseJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Gelen karmaşık JSON yanıtının içinden sadece yapay zekanın yazdığı mesajı çekiyoruz
                dynamic data = JsonConvert.DeserializeObject(responseJson);
                return data.choices[0].message.content.ToString().Trim();
            }
            return "AI Analiz hatası: " + response.StatusCode;
        }
    }
}