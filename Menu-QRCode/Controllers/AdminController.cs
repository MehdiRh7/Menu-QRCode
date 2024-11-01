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
        public IActionResult CreateCategory(Category category, IFormFile ImageUrl)
        {
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img", ImageUrl.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }
                category.ImageUrl = $"/assets/img/{ImageUrl.FileName}";
            }
            _categoryRepository.InsertCategory(category);
            _categoryRepository.Save();

            return RedirectToAction("CategoriesList");
        }


        public IActionResult UpdateCategory(Guid id) 
        {
            return View(_categoryRepository.GetById(id));
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category, IFormFile ImageUrl)
        {
            var cg = _categoryRepository.GetById(category.Id);
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                if (!string.IsNullOrEmpty(cg.ImageUrl))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cg.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img", ImageUrl.FileName);
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }
                cg.ImageUrl = $"/assets/img/{ImageUrl.FileName}";
            }
            _categoryRepository.UpdateCategory(cg);
            _categoryRepository.Save();

            return RedirectToAction("CategoriesList");
        }


        public IActionResult DeleteCategory(Guid id) 
        {
            var category = _categoryRepository.GetById(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult DeleteCategory(Category category) 
        {
            var cg = _categoryRepository.GetById(category.Id);
            if (!string.IsNullOrEmpty(cg.ImageUrl))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cg.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _categoryRepository.DeleteCategory(cg);
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
        public IActionResult CreateMenuItem(MenuItem menuItem, IFormFile ImageUrl)
        {
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/menu", ImageUrl.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }
                menuItem.ImageUrl = $"/assets/img/menu/{ImageUrl.FileName}";
            }
            _menuItemRepository.InsertMenuItem(menuItem);
            _menuItemRepository.Save();
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return RedirectToAction("MenuItemsList");
        }
        public IActionResult UpdateMenuItem(Guid id)
        {
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View(_menuItemRepository.GetById(id));
        }
        [HttpPost]
        public IActionResult UpdateMenuItem(MenuItem menuItem, IFormFile ImageUrl)
        {
            var mi = _menuItemRepository.GetById(menuItem.Id);
            if (!string.IsNullOrEmpty(mi.ImageUrl))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", mi.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            else if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/menu", ImageUrl.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }
                mi.ImageUrl = $"/assets/img/menu/{ImageUrl.FileName}";
            }
            _menuItemRepository.UpdateMenuItem(mi);
            _menuItemRepository.Save();
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return RedirectToAction("MenuItemsList");
        }
        public IActionResult DeleteMenuItem(Guid id)
        {
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View(_menuItemRepository.GetById(id));
        }
        [HttpPost]
        public IActionResult DeleteMenuItem(MenuItem menuItem)
        {
            var mi = _menuItemRepository.GetById(menuItem.Id);
            if (!string.IsNullOrEmpty(mi.ImageUrl))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", mi.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _menuItemRepository.DeleteMenuItem(mi);
            _menuItemRepository.Save();
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return RedirectToAction("MenuItemsList");
        }
    }
}
