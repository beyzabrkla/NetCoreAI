using System;
using System.Linq; // Bunu mutlaka ekle!
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "K88058163188957";

        Console.WriteLine("Resim yolunu giriniz:");
        string rawInput = Console.ReadLine();

        // 1. ADIM: Görünmez karakterleri ve tırnakları kökten temizle
        string imagePath = new string(rawInput.Where(c => c < 128).ToArray()).Trim().Replace("\"", "");

        // 2. ADIM: Dosyanın varlığını kod içinde kontrol et ve kullanıcıya bilgi ver
        if (!File.Exists(imagePath))
        {
            Console.WriteLine("\n--- HATA ---");
            Console.WriteLine($"Girdiğiniz yol: {imagePath}");
            Console.WriteLine("Dosya hala bulunamadı. Lütfen resmi C:\\ klasörüne taşıyıp 'C:\\1.jpg' yazarak deneyin.");
            return;
        }

        Console.WriteLine("\nDosya doğrulandı, API'ye gönderiliyor...");

        try
        {
            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(apiKey), "apikey");
                form.Add(new StringContent("tur"), "language");

                byte[] imageData = File.ReadAllBytes(imagePath);
                form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "file", "image.jpg");

                var response = await httpClient.PostAsync("https://api.ocr.space/parse/image", form);
                var strResponse = await response.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(strResponse);

                if (result.OCRExitCode == 1)
                {
                    Console.WriteLine("\n--- RESİMDEKİ METİN ---");
                    Console.WriteLine(result.ParsedResults[0].ParsedText);
                }
                else
                {
                    Console.WriteLine("API Hatası: " + result.ErrorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Bir sistem hatası oluştu: {ex.Message}");
        }

        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}