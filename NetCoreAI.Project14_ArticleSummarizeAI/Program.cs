using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static readonly string apikey = "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";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Metin Özetleme Paneli ===");

        Console.WriteLine("\nÖzetlenecek makaleyi/metni yapıştırın:");
        string metin = Console.ReadLine();

        if (!string.IsNullOrEmpty(metin))
        {
            Console.WriteLine("\nÜç farklı modda özet hazırlanıyor, lütfen bekleyin...");
            string sonuc = await GenerateSummary(metin);

            Console.WriteLine("\n===========================================");
            Console.WriteLine("            ÖZETLEME RAPORU               ");
            Console.WriteLine("===========================================");
            Console.WriteLine(sonuc);
            Console.WriteLine("===========================================");
        }

        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }

    static async Task<string> GenerateSummary(string text)
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
                        content = "Sen bir özetleme asistanısın. Sana verilen metni şu 3 formatta özetle ve aralarına çizgi koy:\n" +
                                  "1. KISA ÖZET: Tek bir vurucu cümle.\n" +
                                  "2. ORTA ÖZET: Ana fikirleri içeren kısa bir paragraf.\n" +
                                  "3. UZUN ÖZET: Önemli detayları kapsayan geniş anlatım.\n" +
                                  "Dil: Türkçe."
                    },
                    new { role = "user", content = text }
                },
                temperature = 0.4
            };

            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic data = JsonConvert.DeserializeObject(responseJson);
                    return data.choices[0].message.content.ToString().Trim();
                }
                return "API hatası oluştu.";
            }
            catch (Exception ex)
            {
                return "Hata: " + ex.Message;
            }
        }
    }
}