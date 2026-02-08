using Tesseract;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Karakter Okuması Yapılacak Resim Yolu: ");
        string inputPath = Console.ReadLine();

        // Dosya yolunu temizle (Görünmez karakterler ve tırnaklar)
        string imagePath = inputPath.Replace("\u202A", "")
                                     .Replace("\u202B", "")
                                     .Replace("\u202C", "")
                                     .Replace("\u202D", "")
                                     .Replace("\u202E", "")
                                     .Trim('\"', ' ', '?', '\u200B');

        // Fiziksel dosya kontrolü
        if (!File.Exists(imagePath))
        {
            Console.WriteLine($"\nHata: Dosya bulunamadı! Yol: {imagePath}");
            return;
        }

        Console.WriteLine("\nResim işleniyor, lütfen bekleyin...");

        // Tesseract veri dosyalarının yolu
        string tessDataPath = @"C:\tessdata";

        try
        {
            using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath)) //pix resimleri ocr işlemi(görüntü işleme) için uygun hale getiren bir veri yapısı
                {
                    using (var page = engine.Process(img)) //page nesnesi, OCR işlemi sonucunda elde edilen metni temsil eder
                    {
                        string text = page.GetText(); // OCR sonucu elde edilen metni alır

                        Console.WriteLine("\n--- OKUNAN METİN ---"); 
                        if (string.IsNullOrWhiteSpace(text))
                        {
                            Console.WriteLine("Resimde okunabilir bir metin bulunamadı.");
                        }
                        else
                        {
                            Console.WriteLine(text);
                        }
                        Console.WriteLine("--------------------\n");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Hata olursa sebebini yazdırıyoruz
            Console.WriteLine("OCR Hatası: " + ex.Message);
        }

        Console.WriteLine("Çıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}