using DataLayer.Interfaces;
using Menu_QRCode.Models;
using Menu_QRCode.Views.Viewmodels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Menu_QRCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ICategoryRepository _categoryRepository;
        private IMenuItemRepository _menuItemRepository;
        private readonly SignInManager<User> _signInManager;
        public HomeController(ILogger<HomeController> logger,ICategoryRepository categoryRepository,IMenuItemRepository menuItemRepository, SignInManager<User> signInManager)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _menuItemRepository = menuItemRepository;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
        //    User user = new User()
        //    {
        //        UserName = "admin",
        //        PasswordHash = "YWRtaW4xMjM="
        //    };
        //    _signInManager.SignInAsync(user, isPersistent: false);
            Categories_items_Viewmodels item = new Categories_items_Viewmodels();
            item.categories = _categoryRepository.GetAll();
            item.menuItems = _menuItemRepository.GetAll(); 
            return View(item);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
