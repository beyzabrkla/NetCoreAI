using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Speech.Synthesis;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "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".Trim();
        string endpoint = "https://api.groq.com/openai/v1/chat/completions";

        Console.WriteLine("=== AI Sesli Çeviri Sistemi ===");
        Console.Write("Metni girin (TR <-> EN): ");
        string userText = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userText)) return;

        using (var client = new HttpClient()) // HttpClient nesnesi oluşturuluyor
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}"); // API anahtarı ekleniyor

            var requestData = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new {
                        role = "system",
                        content = "Sen profesyonel bir çevirmensin. Türkçe metni İngilizceye, İngilizce metni Türkçeye çevir. Sadece çeviri sonucunu döndür."
                    },
                    new { role = "user", content = userText }
                },
                temperature = 0.3
            };

            string jsonBody = JsonConvert.SerializeObject(requestData); // İstek verisi JSON formatına dönüştürülüyor
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"); // İstek içeriği oluşturuluyor

            try
            {
                var response = await client.PostAsync(endpoint, content); // POST isteği gönderiliyor
                var responseString = await response.Content.ReadAsStringAsync(); // Yanıt içeriği okunuyor

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseString); // JSON yanıtı dinamik olarak ayrıştırılıyor
                    string translatedText = jsonResponse.choices[0].message.content; // Çeviri sonucu alınıyor

                    Console.WriteLine("\n--- ÇEVİRİ ---");
                    Console.WriteLine(translatedText);

                    Console.WriteLine("\nSeslendiriliyor...");
                    using (SpeechSynthesizer synth = new SpeechSynthesizer())
                    {
                        synth.SetOutputToDefaultAudioDevice();

                        // Metnin dili tespit ediliyor
                        bool isTurkish = translatedText.Any(c => "ıİğĞüÜşŞöÖçÇ".Contains(c));

                        // Bilgisayardaki sesleri kontrol et ve uygun olanı seç
                        var voices = synth.GetInstalledVoices();
                        VoiceInfo selectedVoice = null;

                        if (isTurkish)
                        {
                            // Türkçe ses paketini bulmaya çalış (tr-TR)
                            selectedVoice = voices.FirstOrDefault(v => v.VoiceInfo.Culture.Name.StartsWith("tr"))?.VoiceInfo;
                        }
                        else
                        {
                            // İngilizce ses paketini bulmaya çalış (en-US / en-GB)
                            selectedVoice = voices.FirstOrDefault(v => v.VoiceInfo.Culture.Name.StartsWith("en"))?.VoiceInfo;
                        }

                        if (selectedVoice != null)
                        {
                            synth.SelectVoice(selectedVoice.Name);
                            Console.WriteLine($"(Kullanılan Ses: {selectedVoice.Name})");
                        }
                        else
                        {
                            Console.WriteLine("(Uygun dil paketi bulunamadı, varsayılan ses kullanılıyor.)");
                        }

                        synth.Rate = 0;
                        synth.Speak(translatedText);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
        }
        Console.WriteLine("\nİşlem tamamlandı.");
        Console.ReadKey();
    }
}