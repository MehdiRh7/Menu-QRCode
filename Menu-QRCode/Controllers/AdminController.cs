using DataLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Menu_QRCode.Controllers
{
    public class AdminController : Controller
    {
        ICategoryRepository _categoryRepository;
        IMenuItemRepository _menuItemRepository;
        public AdminController(ICategoryRepository categoryRepository,IMenuItemRepository menuItemRepository)
        {
            this._categoryRepository = categoryRepository;
            this._menuItemRepository = menuItemRepository;
        }
        public IActionResult CategoriesList()
        {
            return View(_categoryRepository.GetAll());
        }
        public IActionResult CreateCategory() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category) 
        {
            _categoryRepository.InsertCategory(category);
            _categoryRepository.Save();
            return RedirectToAction("CategoriesList");
        }
        public IActionResult MenuItemsList()
        {
            return View(_menuItemRepository.GetAll());
        }
        public IActionResult CreateMenuItem()
        {
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult CreateMenuItem(MenuItem menuItem)
        {
            _menuItemRepository.InsertMenuItem(menuItem);
            _menuItemRepository.Save();
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return RedirectToAction("MenuItemsList");
        }
    }
}
