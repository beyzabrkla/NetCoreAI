using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

class Program
{
    private static readonly string modelUrl = "https://router.huggingface.co/hf-inference/models/black-forest-labs/FLUX.1-schnell";
    private static readonly string hfToken = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== AI GÖRSEL OLUŞTURUCU (YENİ SİSTEM) ===");

        Console.Write("\nNe çizmek istersiniz? (İngilizce): ");
        string prompt = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(prompt)) return;

        try
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", hfToken);

                Console.WriteLine("\n[⌛] Yeni sistem üzerinden bağlanılıyor, lütfen bekleyin...");

                var payload = new { inputs = prompt };
                string jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(modelUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string fileName = $"AI_Cizim_{DateTime.Now:HHmmss}.png";
                    string fullPath = Path.Combine(desktopPath, fileName);

                    await File.WriteAllBytesAsync(fullPath, imageBytes);

                    Console.WriteLine("\n[✅] BAŞARIYLA ÇİZİLDİ!");
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine($"Görsel Masaüstünde: {fileName}");
                    Console.WriteLine("-------------------------------------------");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("\n[!] Sunucu Hatası: " + response.StatusCode);
                    // Eğer hata mesajı çok uzunsa (HTML gelirse) sadece ilk 100 karakteri yazdır
                    Console.WriteLine("Hata Detayı: " + (error.Length > 100 ? error.Substring(0, 100) + "..." : error));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[!] Program Hatası: " + ex.Message);
        }

        Console.WriteLine("\nKapatmak için bir tuşa basın...");
        Console.ReadKey();
    }
}