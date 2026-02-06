using Google.GenAI;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        var apikey = "AIzaSyC1fwrB923We8BmmhuYCfQPir088iusBi8";

        Console.WriteLine("--- Gemini API (Official SDK) ---");
        Console.Write("Sorunuz: ");
        string soru = Console.ReadLine();

        try
        {
            var client = new Client(apiKey: apikey);

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3-flash-preview", // Hata devam ederse "gemini-1.5-pro" dene
                contents: soru
            );

            if (response.Candidates != null && response.Candidates.Count > 0)
            {
                Console.WriteLine("\nGemini: " + response.Candidates[0].Content.Parts[0].Text);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nHata: {ex.Message}");
        }

        Console.WriteLine("\nKapatmak için bir tuşa basın...");
        Console.ReadKey();
    }
}