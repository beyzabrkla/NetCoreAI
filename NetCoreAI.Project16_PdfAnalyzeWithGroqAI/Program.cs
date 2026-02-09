using Newtonsoft.Json;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

class Program
{
    private static readonly string apikey = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== PDF Analizörü ===");

        // Bilgisayarındaki Masaüstü yolunu otomatik bulur
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        Console.WriteLine($"\n[Sistem] Şu an sadece masaüstüne bakıyorum.");
        Console.Write("Masaüstündeki PDF'in tam adını yazın (Uzantısıyla beraber, örn: makale.pdf): ");

        string fileName = Console.ReadLine().Trim('\"', ' ', '\u202a', '\u202b');

        // Eğer kullanıcı sadece ismi yazarsa .pdf'i biz ekleyelim (Opsiyonel ama garanti)
        if (!fileName.ToLower().EndsWith(".pdf"))
        {
            fileName += ".pdf";
        }

        string fullPath = Path.Combine(desktopPath, fileName);

        if (File.Exists(fullPath))
        {
            try
            {
                Console.WriteLine("\n[✅] DOSYA BULUNDU! Okuma başlıyor...");
                string pdfContent = ExtractTextWithPdfPig(fullPath);

                Console.WriteLine("[AI] Cahit Arf'ın makalesi analiz ediliyor...");
                string aiResponse = await AnalyzeWithAI(pdfContent);

                Console.WriteLine("\n================ ANALİZ SONUCU ================");
                Console.WriteLine(aiResponse);
                Console.WriteLine("===============================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Okuma hatası: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("\n[❌] HATA: DOSYA MASAÜSTÜNDE YOK!");
            Console.WriteLine($"Bakılan Yol: {fullPath}");
            Console.WriteLine("\nLütfen şunları kontrol et:");
            Console.WriteLine("1. Dosya gerçekten masaüstünde mi?");
            Console.WriteLine("2. Dosya adını doğru yazdın mı?");
        }

        Console.WriteLine("\nKapatmak için bir tuşa basın...");
        Console.ReadKey();
    }

    static string ExtractTextWithPdfPig(string path)
    {
        StringBuilder fullText = new StringBuilder();
        using (PdfDocument document = PdfDocument.Open(path))
        {
            foreach (Page page in document.GetPages())
            {
                fullText.Append(page.Text);
                fullText.Append(" ");
            }
        }
        string result = fullText.ToString();
        return result.Length > 6000 ? result.Substring(0, 6000) : result;
    }

    static async Task<string> AnalyzeWithAI(string text)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apikey}");
            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[] {
                    new { role = "system", content = "Sen uzman bir döküman analizcisisin. PDF içeriğindeki ana fikirleri ve Cahit Arf'ın makineler hakkındaki görüşlerini Türkçe özetle." },
                    new { role = "user", content = text }
                }
            };
            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", body);
            dynamic data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            return data.choices[0].message.content;
        }
    }
}