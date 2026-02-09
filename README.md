# 🤖 NetCoreAI Project Showcase
Bu depo, .NET Core ekosistemi üzerinde çeşitli Yapay Zeka (AI) API'leri ve kütüphaneleri kullanılarak geliştirilmiş 20 farklı projeyi içermektedir.

## 🚀 Proje Listesi ve Açıklamalar

### 🌐 01. API Demo
Temel API yapısını ve .NET Core üzerinde bir endpoint'in nasıl oluşturulacağını gösteren başlangıç projesi.

### 🏗️ 02. API Consume UI
Oluşturulan API'lerin arayüz (UI) üzerinden nasıl tüketildiğini ve verilerin nasıl görselleştirildiğini gösteren web tabanlı uygulama.

### ⚡ 03. RapidAPI Integration
RapidAPI platformu üzerindeki harici veri servislerine bağlanarak dinamik veri çekme işlemleri.

### 💬 04. Gemini AI Chat (Official SDK)
Google'ın resmi Google.GenAI kütüphanesi kullanılarak geliştirilmiş bir konsol sohbet asistanıdır.<br>
Kullanılan Model: gemini-3-flash-preview (veya gemini-1.5-pro).<br>
Öne Çıkan Özellik: Harici bir HttpClient yerine doğrudan Google'ın sunduğu nesne yönelimli SDK (Client sınıfı) kullanılmıştır.<br>
Fonksiyon: Kullanıcıdan alınan metin tabanlı soruları yapay zekaya iletir ve gelen yanıtı konsola yazdırır.<br>

### 🎙️ 05. Gemini AI Audio Analysis (Song Analyst)
Ses dosyalarını analiz etmek ve içeriklerini anlamlandırmak için geliştirilmiş bir multimedya projesidir.<br>
Teknoloji: Gemini API Multimodal desteği.<br>
İşleyiş: audio1.mp3 dosyası byte dizisine dönüştürülerek AI modeline InlineData olarak gönderilir.<br>
Fonksiyon: Şarkı sözlerini metne döker (transcription) ve şarkının temasını/konusunu Türkçe olarak analiz eder. Hata durumunda otomatik olarak gemini-1.5-flash modeline geçiş yapan bir hata tolerans mekanizması içerir.<br>

### 🖼️ 06. Hugging Face Image Generation (FLUX.1)
Metin açıklamalarından (prompt) yüksek kaliteli görseller üreten bir yapay zeka sanat uygulamasıdır.<br>
Model: black-forest-labs/FLUX.1-schnell (Hugging Face Inference API).<br>
Teknik Detay: HttpClient üzerinden POST isteği atılarak modelden gelen ham byte verisi işlenir.<br>
Kullanıcı Deneyimi: Kullanıcının girdiği İngilizce betimlemeleri alır, görseli oluşturur ve otomatik olarak kullanıcının Masaüstüne .png formatında, o anki saat bilgisiyle kaydeder.<br>
![6proje](https://github.com/user-attachments/assets/8b10a525-0ed8-43b1-b310-1a26c881c1b3)
![6 2 proje](https://github.com/user-attachments/assets/0d75c6eb-14f6-4fdc-a224-cddf0e4c993e)


### 🔍 07. Tesseract OCR (Local Optical Character Recognition)
Resimlerdeki metinleri yerel bir motor kullanarak dijital metne dönüştüren görüntü işleme projesidir.<br>
Kullanılan Teknoloji: Tesseract.Net.SDK ve Google'ın açık kaynaklı Tesseract OCR Engine.<br>
Öne Çıkan Özellik: Pix.LoadFromFile ile görüntüleri OCR için optimize edilmiş veri yapılarına dönüştürür. tessdata dosyalarını kullanarak internet bağlantısı gerektirmeden çalışabilir.<br>
Fonksiyon: Kullanıcıdan bir resim yolu alır, görünmez karakterleri ve tırnakları temizler, resimdeki İngilizce karakterleri tespit ederek konsola yazdırır.<br>
<img width="1679" height="600" alt="7 proje" src="https://github.com/user-attachments/assets/c177a62d-e91c-4435-af3d-19f1700927f7" />


### 🔍 08. OCR Space Integration (Cloud OCR)
Bulut tabanlı bir API kullanarak, daha yüksek doğruluk oranları ve çok dilli destekle resimden metin ayıklama uygulamasıdır.<br>
Teknoloji: OCR.Space API ve MultipartFormDataContent.<br>
Karşılaştırma: Yerel Tesseract'ın aksine, görselleri buluta göndererek karmaşık yazı tiplerini ve Türkçe dil desteğini (tur) daha başarılı bir şekilde işler.<br>
İşleyiş: HttpClient aracılığıyla resim verilerini byte dizisi olarak API'ye POST eder ve dönen JSON yanıtını Newtonsoft.Json ile ayrıştırır.<br>
<img width="1170" height="389" alt="8 proje" src="https://github.com/user-attachments/assets/549c3f07-fe0f-46a2-b227-97291d088d36" />


### 🌍 09. Groq AI Translate (Smart Language Detector)
Sadece kelime bazlı değil, anlam bazlı çeviri yapan yapay zeka destekli bir tercüman panelidir.<br>
Kullanılan Model: llama-3.3-70b-versatile (Groq API).<br>
Zeka Seviyesi: AI'ya verilen "System Prompt" sayesinde, kullanıcının girdiği dilin Türkçe mi yoksa İngilizce mi olduğunu otomatik tespit eder (Language Detection) ve hedef dile akıcı bir şekilde çevirir.<br>
Teknik Detay: temperature = 0.3 ayarı ile yaratıcılık düşürülerek, çevirinin daha tutarlı ve doğru olması sağlanmıştır. Sadece sonucu döndürmesi için optimize edilmiştir.<br>
<img width="633" height="234" alt="9 proje" src="https://github.com/user-attachments/assets/32346877-42e6-424f-b463-01178543577c" />


### 🔊 10. AI Voice Translation & TTS (Local Synthesis)
Yapay zeka çevirisi ile yerel ses sentezleme teknolojisini birleştiren interaktif bir uygulamadır.<br>
Teknoloji: Groq AI (llama-3.3-70b-versatile) ve System.Speech.Synthesis.<br>
Öne Çıkan Özellik: Uygulama, AI'dan gelen çeviri sonucunu analiz eder; eğer metin Türkçe karakterler (ığüşöç) içeriyorsa sistemdeki Türkçe ses paketini, içermiyorsa İngilizce ses paketini otomatik olarak seçer.<br>
Fonksiyon: Çevrilen metni sadece ekrana yazmakla kalmaz, eş zamanlı olarak kullanıcının varsayılan ses cihazından seslendirir.<br>
<img width="611" height="190" alt="10 proje" src="https://github.com/user-attachments/assets/5cf7f4aa-c35d-4cda-9108-eaf7fd38a3fa" />



### 🎙️ 11. Google Text To Speech (Cloud Based TTS)
Google'ın TTS (Text-to-Speech) servislerini kullanarak yüksek kalitede ses dosyaları üreten bir asistandır.<br>
Yöntem: Google Translate'in gizli API endpoint'ini (translate_tts) HttpClient ile taklit ederek çalışır.<br>
Akıllı Dil Tespiti: Regex ([ğĞüÜşŞİıçÇöÖ]) kullanarak metnin dilini saniyeler içinde tespit eder ve doğru aksanla seslendirilmesini sağlar.<br>
Fonksiyon: Oluşturulan sesi bir output.mp3 dosyası olarak kaydeder ve Process.Start komutu ile dosyayı otomatik olarak varsayılan medya oynatıcısında açar.<br>
<img width="507" height="433" alt="11 proje" src="https://github.com/user-attachments/assets/44f19994-36cb-479f-9385-116b37ef41f0" />


### 📊 12. Sentiment AI App (Sentiment Analysis)
Metinlerin ardındaki duygusal tonu analiz eden yapay zeka tabanlı bir sınıflandırma uygulamasıdır.<br>
Kullanılan Model: Groq API üzerinden llama-3.3-70b-versatile.<br>
Teknik Detay: AI modeline verilen "System Prompt" kısıtlaması sayesinde, model gereksiz açıklama yapmadan sadece "Pozitif", "Negatif" veya "Nötr" etiketlerinden birini döndürür.<br>
Fonksiyon: Kullanıcı geri bildirimlerini veya sosyal medya yorumlarını analiz etmek için ideal olan bu sistem, metni işleyerek duygu durumunu hızlıca tespit eder.<br>
<img width="1070" height="615" alt="12 proje" src="https://github.com/user-attachments/assets/acf01470-7a0f-4add-84c0-e7180363d424" />


### 📊 13. Sentiment With Degree AI (Deep Emotional Analysis)
Basit duygu analizinin ötesine geçerek, metindeki duygusal yoğunluğu matematiksel verilere döken bir analiz projesidir.<br>
Kullanılan Model: Groq API üzerinden llama-3.3-70b-versatile.<br>
Uzmanlık Alanı: AI, bir psikolog ve dil bilimci rolünü üstlenerek metni beş farklı kategoride (Mutluluk, Öfke, Üzüntü, Şaşkınlık, Nötr) inceler.<br>
Fonksiyon: Analiz sonucunu toplamı %100 olacak şekilde bir yüzdelik rapor halinde sunar. Kullanıcının ruh halini verilere dayalı olarak takip etmek için mükemmel bir temel oluşturur.<br>
<img width="1098" height="411" alt="13 proje" src="https://github.com/user-attachments/assets/23dd9550-cff7-49ab-b18c-fc79d185fc9e" />



### 📝 14. Article Summarize AI (Three-Tier Summary Mode)
Uzun metinleri ve akademik makaleleri üç farklı derinlikte özetleyen bir verimlilik aracıdır.<br>
Özellik: Tek bir girdi ile AI'dan üç farklı çıktı alınır:<br>
Kısa Özet: Tek cümlelik ana fikir.<br>
Orta Özet: Ana fikirleri içeren profesyonel bir paragraf.<br>
Uzun Özet: Detayları ve önemli noktaları kapsayan geniş anlatım.<br>
Kullanım Amacı: Kullanıcının vaktine göre hangi özeti okumak istediğini seçmesine olanak tanır, bilgiye erişimi hızlandırır.<br>
<img width="987" height="486" alt="14 proje" src="https://github.com/user-attachments/assets/8dbf70c0-3af8-4223-9b01-48fc014d3a19" />



### 🌐 15. Web Scraping With Groq AI (Intelligent Data Extraction)
Web sitelerindeki karmaşık ve gürültülü verileri ayıklayarak anlamlı bilgilere dönüştüren bir veri kazıma (scraping) projesidir.<br>
Teknoloji: HtmlAgilityPack kütüphanesi ve Groq AI entegrasyonu.<br>
İşleyiş: Belirtilen URL'deki HTML kodlarını indirir, XPath kullanarak sadece <h1, h2, h3, p> gibi metinsel etiketleri filtreler.<br>
AI Analizi: Ayıklanan ham metin (clean text), yapay zekaya gönderilir. AI, bu karmaşık metin içinden en önemli bilgileri, ürün detaylarını veya başlıkları seçerek düzenli bir Türkçe liste hazırlar.<br>
Teknik Detay: Modelin token limitini aşmamak için metin otomatik olarak 4000 karakterle sınırlandırılmıştır.<br>
<img width="1919" height="788" alt="15 proje" src="https://github.com/user-attachments/assets/d7ba5f1a-e491-4075-95a7-68a11c3a1beb" />


### 📂 16. PDF Analyze With Groq AI (Cahit Arf Special)
Statik PDF dosyalarını dijital metne dönüştürerek içerik analizi yapan bir döküman asistanıdır.<br>
Teknoloji: UglyToad.PdfPig kütüphanesi ve Groq AI (llama-3.3-70b-versatile).<br>
Özellik: Kullanıcının masaüstündeki PDF dosyasını otomatik olarak bulur. Metni sayfa sayfa tarayarak anlamlı bir bütün haline getirir.<br>
Fonksiyon: Proje örneğinde Türk matematikçi Cahit Arf'ın "Makineler Düşünebilir mi?" üzerine makalesi analiz edilmektedir. AI, dökümandaki karmaşık fikirleri sentezleyerek ana fikirleri özetler.<br>
Sınırlandırma: Model kapasitesini aşmamak adına PDF'in ilk 6000 karakteri işlenmektedir.<br>
<img width="1915" height="538" alt="16 proje" src="https://github.com/user-attachments/assets/591fc93a-e4a3-4fce-af99-485a7b8a3a46" />



### 🖼️ 17. Hugging Face Image Captioning (Görsel Analiz)
Yüklenen görsellerin içeriğini yapay zeka aracılığıyla anlayan ve "gördüğünü" metne döken bir sistemdir.<br>
Model: Salesforce/blip-image-captioning-large (Hugging Face Inference API).<br>
Teknik Detay: Görsel, ByteArrayContent olarak binary formatta API'ye iletilir. Hugging Face Router üzerinden en uygun modele yönlendirilir.<br>
Fonksiyon: Kullanıcı masaüstündeki bir görseli belirtir; sistem bu görseli analiz eder ve içeriği (Örn: "sahilde koşan bir köpek") betimleyen bir yanıt döndürür. Proje, hata yönetimi (410/404 durumları) konusunda detaylı bir çıktı sistemine sahiptir.<br>


### 📰 18. Hugging AI News Summarize (RSS News Agent)
Güncel haber kaynaklarından anlık veri çekip özetleyen bir otonom haber ajansı simülasyonudur.<br>
Teknoloji: System.ServiceModel.Syndication ve Facebook'un bart-large-cnn özetleme modeli.<br>
İşleyiş: Bloomberg HT gibi RSS kaynaklarına bağlanarak son 10 haberi çeker. Haber içeriklerindeki HTML etiketlerini Regex ile temizler.<br>
Akıllı Özetleme: Her bir haberi tek tek Hugging Face API'sine gönderir. min_length ve max_length parametreleri sayesinde haberleri okuyucuyu yormayacak kıvamda (20-80 kelime arası) özetler.<br>
Otomasyon: API limitlerini korumak için her haber işleme süreci arasına 1 saniyelik gecikme (Task.Delay) eklenmiştir.<br>
<img width="1212" height="965" alt="18 proje" src="https://github.com/user-attachments/assets/0a220e85-37a0-41c2-9d66-12e4c890a2ba" />



### ✍️ 19. AI Story Generator (Creative Writing)
Kullanıcının verdiği parametrelerle tamamen özgün ve edebi değeri yüksek hikayeler kurgulayan bir yaratıcı yazarlık aracıdır.<br>
Teknoloji: Groq API (llama-3.1-8b-instant) ve System.Text.Json.<br>
Kreatif Kontrol: temperature = 0.8 parametresi kullanılarak yapay zekanın daha yaratıcı, beklenmedik ve akıcı cümleler kurması sağlanmıştır.<br>
Fonksiyon: Karakter, mekan ve tür bilgilerini harmanlayarak giriş, gelişme ve sonuç bölümlerinden oluşan, derinliği olan Türkçe metinler üretir.<br>
<img width="1253" height="900" alt="19 proje" src="https://github.com/user-attachments/assets/955a8423-bfd1-4a44-8306-9b9e8030ab50" />



### 🍳 20. AI Chef: Recipe & Nutrition Assistant (Final Project)
Bu proje, evdeki malzemelerle neler yapılabileceğini söyleyen veya istenen yemeğin tarifini adım adım veren bir mutfak asistanıdır.<br>
Hibrit Mantık: Kullanıcı sadece malzeme listesi (Örn: "Yumurta, domates, peynir") girerse AI bir "Yaratıcı Mutfak" moduyla tarif üretir. 
Teknik Yapı: En güncel llama-3.3-70b modeli kullanılarak gastronomi konusundaki detaylara hakimiyet artırılmıştır.<br>
<img width="1167" height="760" alt="20-1 proje" src="https://github.com/user-attachments/assets/f1d816b9-7926-4da5-b04d-a67a0a749bd1" />
<img width="1290" height="890" alt="20-2 proje" src="https://github.com/user-attachments/assets/58afe654-cdb1-4b08-b937-195aca68d0d4" />



