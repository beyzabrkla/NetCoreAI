using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static readonly string apikey = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Duygu Yoğunluğu ve Analiz Oranı ===");
        Console.Write("\nAnaliz edilecek metni girin: ");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine("\nYapay Zeka derin analiz yapıyor, lütfen bekleyin...");
            string result = await AnalyzeEmotionalDensity(input);

            Console.WriteLine("\n-------------------------------------------");
            Console.WriteLine("DUYGU ANALİZ RAPORU");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(result);
            Console.WriteLine("-------------------------------------------");
        }

        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }

    static async Task<string> AnalyzeEmotionalDensity(string text)
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
                        content = "Sen bir psikolog ve dil bilimci gibi analiz yapıyorsun. Verilen metni şu 4 kategoriye ayır: Mutluluk, Öfke, Üzüntü, Şaşkınlık ve Nötr. " +
                                  "Sonuçları sadece yüzdelik oran olarak ve her biri yeni satırda olacak şekilde şu formatta ver: " +
                                  "Mutluluk: %X\n Öfke: %X\n Üzüntü: %X\n Nötr: %X\n Şaşırma: %X\n \nToplam her zaman %100 olmalı."
                    },
                    new { role = "user", content = $"Şu metni analiz et: \"{text}\"" }
                },
                temperature = 0.2 // Tutarlılığı artırır
            };

            string json = JsonConvert.SerializeObject(requestBody); //metin json formatına çevirilir
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic data = JsonConvert.DeserializeObject(responseJson); //json dosyası parçalanır
                    return data.choices[0].message.content.ToString().Trim();
                }
                else
                {
                    return "Hata: API yanıt vermedi. " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                return "Bağlantı Hatası: " + ex.Message;
            }
        }
    }
}