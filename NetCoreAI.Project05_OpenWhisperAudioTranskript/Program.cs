using Google.GenAI;
using Google.GenAI.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var apikey = "AIzaSyC1fwrB923We8BmmhuYCfQPir088iusBi8";
        string audioFilePath = "audio1.mp3";

        Console.WriteLine("--- Gemini API - Şarkı Analizi ---\n");

        try
        {
            // Ses dosyasını oku
            byte[] audioBytes = await System.IO.File.ReadAllBytesAsync(audioFilePath);
            Console.WriteLine("Ses dosyası okundu. İşleniyor...\n");

            var client = new Client(apiKey: apikey);

            // İçerik oluştur
            var requestContents = new List<Content>
            {
                new Content {
                    Role = "user",
                    Parts = new List<Part>
                    {
                        new Part { Text = "Bu sevdiğim bir sanatçının şarkısı. Lütfen şarkıdaki sözleri yazıya dök ve konusunu Türkçe açıkla." },
                        new Part {
                            InlineData = new Blob {
                                MimeType = "audio/mpeg",
                                Data = audioBytes
                            }
                        }
                    }
                }
            };

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3-flash-preview",
                contents: requestContents
            );

            if (response.Candidates != null && response.Candidates.Count > 0)
            {
                Console.WriteLine("=== SONUÇ ===\n");
                Console.WriteLine(response.Candidates[0].Content.Parts[0].Text);
            }
            else
            {
                Console.WriteLine("Yanıt alınamadı.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nHata: {ex.Message}");
            Console.WriteLine("\n--- Alternatif model deneniyor: gemini-1.5-flash ---\n");

            // Alternatif model dene
            try
            {
                byte[] audioBytes = await System.IO.File.ReadAllBytesAsync(audioFilePath);
                var client = new Client(apiKey: apikey);

                var requestContents = new List<Content>
                {
                    new Content {
                        Role = "user",
                        Parts = new List<Part>
                        {
                        new Part { Text = "Bu sevdiğim bir sanatçının şarkısı. Lütfen şarkıdaki sözleri yazıya dök ve konusunu Türkçe açıkla." },
                            new Part {
                                InlineData = new Blob {
                                    MimeType = "audio/mpeg",
                                    Data = audioBytes
                                }
                            }
                        }
                    }
                };

                var response = await client.Models.GenerateContentAsync(
                    model: "gemini-1.5-flash",
                    contents: requestContents
                );

                if (response.Candidates != null && response.Candidates.Count > 0)
                {
                    Console.WriteLine("=== SONUÇ ===\n");
                    Console.WriteLine(response.Candidates[0].Content.Parts[0].Text);
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine($"Alternatif model de başarısız: {ex2.Message}");
            }
        }

        Console.WriteLine("\n\nKapatmak için bir tuşa basın...");
        Console.ReadKey();
    }
}