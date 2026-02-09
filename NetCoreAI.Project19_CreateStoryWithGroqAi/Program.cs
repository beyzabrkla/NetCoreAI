using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

class Program
{
    private static readonly string apiUrl = "https://api.groq.com/openai/v1/chat/completions";
    private static readonly string apiKey = "";

    static async Task Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("=== AI HİKAYE OLUŞTURUCU (GROQ SPEED) ===\n");

        Console.Write("Karakter: "); string karakter = Console.ReadLine();
        Console.Write("Mekan: "); string mekan = Console.ReadLine();
        Console.Write("Tür: "); string tur = Console.ReadLine();

        Console.WriteLine("\n[⏳] AI Yanıtı Bekleniyor (Llama 3.1)...");

        string story = await GenerateStory(karakter, mekan, tur);

        Console.WriteLine("\n[📜] OLUŞTURULAN HİKAYE:");
        Console.WriteLine("----------------------------------------------------");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(story);
        Console.ResetColor();
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }

    static async Task<string> GenerateStory(string karakter, string mekan, string tur)
    {
        using var client = new HttpClient();
        // DÜZELTME: DefaultHeaders değil DefaultRequestHeaders olmalı
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        // Groq için gerekli model ve mesaj yapısı
        var payload = new
        {
            model = "llama-3.1-8b-instant",
            messages = new[] {
            new {
                role = "system",
                content = "Sen usta bir Türk yazarsın. Karakterleri ve mekanı derinlemesine analiz ederek, edebi ve akıcı bir hikaye kurgularsın."
            },
            new {
                role = "user",
                content = $"Karakterler: {karakter}\nMekan: {mekan}\nTür: {tur}\n\nBu unsurları kullanarak, giriş, gelişme ve sonuç bölümleri olan, duygusal derinliği yüksek, detaylı ve uzun bir Türkçe hikaye yaz."
            }
        },
            temperature = 0.8,
            max_tokens = 2000
        };

        try
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(result);
                return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            }

            return $"API Hatası: {response.StatusCode}\nDetay: {result}";
        }
        catch (Exception ex)
        {
            return $"Bağlantı Hatası: {ex.Message}";
        }
    }
}