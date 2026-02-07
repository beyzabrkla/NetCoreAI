using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Hugging Face API Key (Token geçerli görünüyor)
        string hfToken = "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";

        // 2. En güncel ve stabil çalışan model URL'si
        string modelUrl = "https://router.huggingface.co/hf-inference/models/stabilityai/stable-diffusion-xl-base-1.0";

        Console.Write("Ne çizmemi istersiniz? : ");
        string prompt = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(prompt)) return;

        try
        {
            using (var client = new HttpClient())
            {
                // Zaman aşımını uzatıyoruz çünkü görsel oluşturma vakit alır
                client.Timeout = TimeSpan.FromMinutes(3);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", hfToken);

                Console.WriteLine("\n[1/2] Görsel oluşturuluyor (Bu işlem 10-60 saniye sürebilir)...");
                Console.WriteLine("Not: Eğer model yüklü değilse ilk istek biraz uzun sürebilir.");

                // API'ye isteği JSON string olarak manuel gönderiyoruz
                var jsonPayload = $"{{\"inputs\": \"{prompt}\"}}";
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(modelUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                    string dosyaAdi = "huggingface_cizim.png";
                    await File.WriteAllBytesAsync(dosyaAdi, imageBytes);

                    Console.WriteLine("\n[2/2] BAŞARILI!");
                    Console.WriteLine($"Görsel kaydedildi: {Path.GetFullPath(dosyaAdi)}");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("\nHata oluştu: " + response.StatusCode);

                    if (error.Contains("estimated_time"))
                    {
                        Console.WriteLine("Bilgi: Model şu an yükleniyor, lütfen 20 saniye sonra tekrar deneyin.");
                    }
                    else
                    {
                        Console.WriteLine("Detay: " + error);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nBeklenmedik bir hata: " + ex.Message);
        }

        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}