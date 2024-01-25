using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MVCIntroDemo.Models.Product;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace MVCIntroDemo.Controllers
{
    public class ProductController : Controller
    {
        private IEnumerable<ProductViewModel> _products = new List<ProductViewModel>()
        {
            new ProductViewModel()
            {
                Id = 1,
                Name = "Cheese",
                Price = 15
            },
        new ProductViewModel
        {
            Id= 2,
            Name ="Ham",
            Price=25
        },
        new ProductViewModel 
        { 
            Id= 3,
            Name = "Bread",
            Price= 2.8m
        }
        };
        [ActionName("My-products")]
       public IActionResult All(string keyword)
        {
            if (keyword != null)
            {
                var result = _products.Where(x => x.Name.ToLower().Contains(keyword.ToLower()));
                return View(result);
            }
            return View(_products); 
        }
        public IActionResult ById(int id) 
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return BadRequest();
            }
            return View(product);
        }
        public IActionResult AllAsJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return Json(_products, options );
        }
        public IActionResult AllAsPlainText()
        {
            var text = string.Empty;
            foreach (var item in _products)
            {
                text += $"Product {item.Id}: {item.Name} - {item.Price} lv.";
                text += "\r\n";
            }
            return Content(text);
        }
        public IActionResult AllAsTextFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _products)
            {
                sb.AppendLine($"Product: {item.Id}: {item.Name} - {item.Price:f2} lv.");
            }
            Response.Headers.Add(HeaderNames.ContentDisposition, @"attachment;filename=products.txt");
            return File(Encoding.UTF8.GetBytes(sb.ToString().TrimEnd()), "text/plain");
        }
    }
}
