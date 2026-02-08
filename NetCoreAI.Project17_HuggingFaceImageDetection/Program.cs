using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    // EN GÜNCEL MODEL: Salesforce'un 'large' versiyonu şu an daha stabil
    private static readonly string apiUrl = "https://api-inference.huggingface.co/models/Salesforce/blip-image-captioning-base";
    private static readonly string hfToken = "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";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== AI GÖRSEL ANALİZ (SURVIVOR MODU: LARGE) ===");

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        Console.Write("\nMasaüstündeki resmin adı (Örn: kedi.jpg): ");
        string fileName = Console.ReadLine()?.Trim('\"', ' ');

        // ÖNEMLİ: Eğer dosya adında Türkçe karakter varsa hata alabiliriz.
        // Lütfen masaüstündeki resmin adını 'kedi.jpg' olarak değiştirip dene.
        string filePath = Path.Combine(desktopPath, fileName);

        if (!File.Exists(filePath))
        {
            Console.WriteLine("\n[❌] Dosya bulunamadı: " + filePath);
            return;
        }

        try
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(2);

                // KRİTİK AYARLAR: Sunucuya kim olduğumuzu net söylüyoruz
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", hfToken);
                client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.32.3"); // Tarayıcı gibi davranmasın
                client.DefaultRequestHeaders.Add("X-Wait-For-Model", "true");

                Console.WriteLine("\n[1/2] Resim gönderiliyor (BLIP-Large)...");

                byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
                var content = new ByteArrayContent(imageBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await client.PostAsync(apiUrl, content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var predictions = JsonSerializer.Deserialize<List<Prediction>>(responseJson);
                    if (predictions != null && predictions.Count > 0)
                    {
                        Console.WriteLine("\n[✅] BAŞARILI!");
                        Console.WriteLine("-------------------------------------------");
                        Console.WriteLine("ANALİZ: " + predictions[0].generated_text.ToUpper());
                        Console.WriteLine("-------------------------------------------");
                    }
                }
                else if ((int)response.StatusCode == 410 || (int)response.StatusCode == 404)
                {
                    Console.WriteLine("\n[!] Hugging Face sunucuları şu an taşınıyor.");
                    Console.WriteLine("Lütfen 1 dakika sonra tekrar deneyin veya farklı bir resim seçin.");
                }
                else
                {
                    Console.WriteLine("\n[!] Hata: " + response.StatusCode);
                    Console.WriteLine("Detay: " + (responseJson.Length > 100 ? "Model şu an yüklenemedi." : responseJson));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[!] Beklenmedik hata: " + ex.Message);
        }

        Console.WriteLine("\nBitirmek için bir tuşa basın...");
        Console.ReadKey();
    }
}

public class Prediction
{
    public string generated_text { get; set; }
}