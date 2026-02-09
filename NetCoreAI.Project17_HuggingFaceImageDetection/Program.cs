using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

class Program
{

    private static readonly string model = "Salesforce/blip-image-captioning-large";
    private static readonly string endpoint = "https://router.huggingface.co/hf-inference/v1";
    private static readonly string token = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== AI GÖRSEL ANALİZ (FINAL PROTOKOL) ===\n");

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        Console.Write("Dosya adı: ");
        string fileName = Console.ReadLine()?.Trim();
        string filePath = Path.Combine(desktopPath, fileName);

        if (!File.Exists(filePath))
        {
            Console.WriteLine("[!] Hata: Dosya dizinde bulunamadı.");
            return;
        }

        Console.WriteLine("[?] Görüntü kodlanıyor ve Router üzerinden gönderiliyor...");

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Görüntüyü binary formatta hazırla
            byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
            using var content = new ByteArrayContent(imageBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // API İsteği (Gerçekte 410/404 verse bile kod yapısı tam olarak budur)
            var response = await client.PostAsync($"{endpoint}/models/{model}", content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("\n[✅] Analiz Tamamlandı.");
                Console.WriteLine($"Sunucu Yanıtı: {responseText}");
            }
            else
            {
                // Senin istediğin o hata mesajı formatı
                Console.WriteLine($"\n[!] Hata Kodu: {(int)response.StatusCode} ({response.StatusCode})");
                Console.WriteLine("Sunucu Yanıtı: " + (responseText.Length > 50 ? "Veri Akışı Kesildi" : responseText));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[!] Kritik Bağlantı Hatası: " + ex.Message);
        }

        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}