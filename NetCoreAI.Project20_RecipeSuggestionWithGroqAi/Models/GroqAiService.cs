using System.Text;
using System.Text.Json;
using System.Net.Http.Headers; // AuthorizationValue için gerekli

namespace NetCoreAI.Project20_RecipeSuggestionWithGroqAi.Models
{
    public class GroqAiService
    {
        private readonly HttpClient _httpClient;
        private const string GroqAiUrl = "https://api.groq.com/openai/v1/chat/completions";
        private const string ApiKey = "";

        public GroqAiService()
        {
            _httpClient = new HttpClient();
            // Düzenleme: Header ekleme yöntemini modernize ettik
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        }

        public async Task<string> GetRecipeAsync(string ingredients)
        {
            var requestBody = new
            {
                model = "llama-3.1-8b-instant",
                messages = new[]
                {
                    new {
                        role = "system", 
                        // Talimatı netleştirdik: Türkçe karakter ve liste formatı vurgusu
                        content = "Sen profesyonel bir Türk aşçısın. Sadece verilen malzemeleri kullanarak, adım adım, Türkçe dil kurallarına (ğ, ü, ş, ı, ö, ç) uygun ve HTML formatında güzel görünecek bir tarif hazırlarsın."
                    },
                    new {
                        role = "user",
                        content = $"Elimdeki malzemeler şunlar: {ingredients}. Bu malzemelerle yapılabilecek, başlığı olan, detaylı bir yemek tarifi yazar mısın?"
                    }
                },
                temperature = 0.7,
                max_tokens = 1500 // Yanıtın yarım kalmaması için sınırı artırdık
            };

            try
            {
                var jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(GroqAiUrl, content);

                // Karakter bozulmalarını önlemek için yanıtı UTF8 ile oku
                var bytes = await response.Content.ReadAsByteArrayAsync();
                var jsonResponse = Encoding.UTF8.GetString(bytes);

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(jsonResponse);
                    string recipe = doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    return recipe ?? "Tarif oluşturulamadı.";
                }

                return $"Bir hata oluştu: {response.StatusCode} - {jsonResponse}";
            }
            catch (Exception ex)
            {
                return $"Bağlantı hatası: {ex.Message}";
            }
        }
    }
}