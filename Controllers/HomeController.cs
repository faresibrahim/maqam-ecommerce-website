using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicShoppingCartMvcUI.Models;
using MusicShoppingCartMvcUI.Models.DTOs;

namespace MusicShoppingCartMvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
        

        public HomeController(ILogger<HomeController> logger , IHomeRepository homeRepository)
        {
            //inject homerepository to the home controller
            _homeRepository = homeRepository;
            _logger = logger;
            
        }

        public async Task<IActionResult> Index(string sTerm="", int categoryId=0)
        {
            
            IEnumerable<Product> products = await _homeRepository.DisplayProducts(sTerm,categoryId);
            IEnumerable<Category> categories = await _homeRepository.DisplayCategories();
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = products,
                Categories = categories,
                STerm = sTerm,
                CategoryId= categoryId
            };

            return View(productModel);
        }


        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";
            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Landing()
        {
            var result = await _homeRepository.DisplayCategories();
            return View(result);
        }
    }
}
