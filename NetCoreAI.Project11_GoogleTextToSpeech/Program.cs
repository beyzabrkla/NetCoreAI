using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Akıllı Sesli Asistan (TR/EN) ===");
            Console.Write("\nSeslendirilecek metni girin: ");
            string input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Ses dosyası oluşturuluyor...");
                await GenerateSpeech(input);
            }
        }

        static async Task GenerateSpeech(string text)
        {
            try
            {
                string fileName = "output.mp3";
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                string lang = Regex.IsMatch(text, @"[ğĞüÜşŞİıçÇöÖ]") ? "tr" : "en";

                using (HttpClient client = new HttpClient())
                {
                    // Tarayıcı gibi tanıtıyoruz
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                    string url = $"https://translate.google.com/translate_tts?ie=UTF-8&q={Uri.EscapeDataString(text)}&tl={lang}&client=tw-ob";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();
                        await File.WriteAllBytesAsync(fullPath, audioBytes);

                        Console.WriteLine($"\n[BAŞARILI] {lang.ToUpper()} diliyle ses oluşturuldu.");

                        Process.Start("explorer.exe", fileName);
                    }
                    else
                    {
                        Console.WriteLine("Sunucu hatası: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sistem Hatası: " + ex.Message);
            }
        }
    }