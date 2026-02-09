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
        Console.Write("Lütfen metni giriniz: ");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input)) 
        {
            Console.WriteLine("\n[AI] Duygu Analizi Yapılıyor....");
            string sentiment = await AnalyzeSentiment(input);
            Console.WriteLine($"\nSonuç: {sentiment}");
        }
    }

    static async Task<string> AnalyzeSentiment(string text)
    {
        using (HttpClient client = new HttpClient()) 
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apikey}");

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = "Sen duygu analizi yapan bir asistansın. Sadece 'Pozitif', 'Negatif' veya 'Nötr' şeklinde cevap vereceksin." },
                    new { role = "user", content = $"Bu metnin duygusunu analiz et: \"{text}\"" }
                 }
            };

            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

            string responseJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                dynamic result = JsonConvert.DeserializeObject(responseJson);
                return result.choices[0].message.content.ToString().Trim();
            }
            else
            {
                Console.WriteLine("Hata oluştu: " + responseJson);
                return "Hata";
            }
        }
    }
}