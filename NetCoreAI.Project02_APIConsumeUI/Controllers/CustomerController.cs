using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreAI.Project02_APIConsumeUI.DTOs;
using Newtonsoft.Json;

namespace NetCoreAI.Project02_APIConsumeUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> CustomerList()
        {
            var client = _httpClientFactory.CreateClient(); //istek oluşturuldu
            var responseMessage = await client.GetAsync("https://localhost:7008/api/Customers"); //api adresinden mesaj çekme işlemi yapıldı
            if (responseMessage.IsSuccessStatusCode) // mesaj içeriği doluysa 
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); //mesajın içindeki içeriği json verisi olarak atıyoruz
                var values = JsonConvert.DeserializeObject<List<ResultCustomerDTO>>(jsonData); //value değişkeni veriyi deserialize ediyor yani gelen json verisini string formatına dönüştürüyor.
                                                                                               //ResultCustomer ile api adresinden gelen veri eşleştiriliyor.
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDTO createCustomerDTO)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createCustomerDTO); //gelen dto verisi(string veriyi) json formatına dönüştürülüyor
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json"); //oluşturulan json verisi stringcontent e dönüştürülüyor
            var responseMessage = await client.PostAsync("https://localhost:7008/api/Customers", stringContent); //api adresine post işlemi yapılıyor
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomerList");
            }
            return View();
        }


        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var client = _httpClientFactory.CreateClient();//istek oluşturuldu
            var responseMessage = await client.DeleteAsync($"https://localhost:7008/api/Customers/?id=" + id); //api adresinden silinmesi istenen id ile istek atıldı
            if (responseMessage.IsSuccessStatusCode) // id başarılı bir şekilde silindiyse
            {
                return RedirectToAction("CustomerList");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7008/api/Customers/GetCustomer?id=" + id); //güncellenmek istenen id ile istek atıldı
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); //mesajın içindeki içeriği json verisi olarak atıyoruz
                var values = JsonConvert.DeserializeObject<GetByIdCustomerDTO>(jsonData); //gelen json verisi stringe çevirilip GetByIdCustomerDTO ya gönderiliyor
                return View(values);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDTO updateCustomerDTO)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateCustomerDTO); //gelen dto verisi(string veriyi) json formatına dönüştürülüyor
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json"); //oluşturulan json verisi stringcontent e dönüştürülüyor
            var responseMessage = await client.PutAsync("https://localhost:7008/api/Customers", stringContent); //api adresine put(güncelleme) işlemi yapılıyor
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomerList");
            }
            return View();
        }
    }
}
