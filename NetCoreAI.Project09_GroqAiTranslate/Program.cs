using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // Bilgileri tanımlıyoruz
        string apiKey = "".Trim(); //boşluklar varsa kaldırıyoruz
        string endpoint = "https://api.groq.com/openai/v1/chat/completions";

        Console.WriteLine("--- Groq AI Çeviri Paneli ---");
        Console.Write("Çevrilecek metni girin: ");
        string userText = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userText)) return; // Eğer kullanıcı boş bir metin girdiyse programı sonlandır

        using (var client = new HttpClient()) // HttpClient'ı kullanarak API'ye istek göndermek için oluşturuyoruz
        {
            client.DefaultRequestHeaders.Clear(); //api onaylamak için header'ları temizliyoruz , mektup hazırlığı gibi düşünebiliriz
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}"); // API anahtarını header'a ekliyoruz, bearer bu anahtarı taşıyan anlamına gelir 

            // İstek objesi
            var requestData = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new {
                        role = "system",
                        content = "Sen zeki bir dil tespit edici ve çevirmensin. Eğer kullanıcı Türkçe yazarsa onu akıcı bir İngilizceye çevir. Eğer kullanıcı İngilizce yazarsa onu doğal bir Türkçeye çevir. Sadece çeviri sonucunu döndür, başka açıklama yapma."
                    },
                new { role = "user", content = userText }
                    },
                        temperature = 0.3 // Daha tutarlı çeviriler için
                    };

            string jsonBody = JsonConvert.SerializeObject(requestData); // İstek verilerini JSON formatına dönüştürüyoruz
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine("\nGroq AI yanıt veriyor...");

                // POST isteği - URL ve içeriği açıkça gönderiyoruz
                var response = await client.PostAsync(endpoint, content); // API'ye POST isteği gönderiyoruz ve yanıtı bekliyoruz
                var responseString = await response.Content.ReadAsStringAsync(); // API'den gelen yanıtı string olarak okuyoruz

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseString); // Gelen JSON yanıtını dinamik bir nesneye dönüştürüyoruz
                    string translatedText = jsonResponse.choices[0].message.content; // Çeviriyi JSON yanıtından alıyoruz

                    Console.WriteLine("\n--- ÇEVİRİ SONUCU ---");
                    Console.WriteLine(translatedText);
                }
                else
                {
                    // Hata olursa burası çalışacak ve nedenini söyleyecek
                    Console.WriteLine("\n--- SUNUCU HATASI ---");
                    Console.WriteLine(responseString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nBir program hatası oluştu: " + ex.Message);
            }
        }

        Console.WriteLine("\nÇıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}