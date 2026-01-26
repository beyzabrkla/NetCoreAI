using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project01_APIDemo.Context;
using NetCoreAI.Project01_APIDemo.Entities;

namespace NetCoreAI.Project01_APIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApiContext _context;

        public CustomersController(ApiContext context)
        {
            _context = context;
        }


        [HttpGet]  //veri listeleme işlemi
        public IActionResult CustomerList()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers); //mesaj kutusunun içine value değerleri gelsin 
        }

        [HttpPost] //veri ekleme işlemi
        public IActionResult CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return Ok("Müşteri Ekleme İşlemi Başarılı !");
        }

        [HttpDelete]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound("Müşteri Bulunamadı !");
            }
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return Ok("Müşteri Silme İşlemi Başarılı !");
        }

        [HttpGet("GetCustomer")] //get customer olarak isim veriyoruz ilk metottan ayrılsın
        public IActionResult GetCustomer(int id) //aynı isimli metotlar olursa hata verir
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound("Müşteri Bulunamadı !");
            }
            return Ok(customer);
        }

        [HttpPut] //Güncelleme işlemi yapar
        public IActionResult UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return Ok("Müşteri Güncelleme İşlemi Başarılı !");
        }
    }
}
