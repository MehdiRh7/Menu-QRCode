using DataLayer.Interfaces;
using DataLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Text;

namespace Menu_QRCode.Controllers
{
    public class AdminController : Controller
    {
        ICategoryRepository _categoryRepository;
        IMenuItemRepository _menuItemRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager,ICategoryRepository categoryRepository, IMenuItemRepository menuItemRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._categoryRepository = categoryRepository;
            this._menuItemRepository = menuItemRepository;
        }
        
        public async Task<IActionResult> Login()
        {
           
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CategoriesList", "Admin");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                if (user.Role == "Admin")
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    HttpContext.Response.Cookies.Append("AdminAccess", "true", new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true
                    });

                    return RedirectToAction("CategoriesList", "Admin");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "شما دسترسی به بخش ادمین ندارید.");
                    return View();
                }
            }

            ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور اشتباه است.");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CategoriesList()
        {
            return View(_categoryRepository.GetAll());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory() 
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

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
            if(category.Description == null)
            {
                category.Description = string.Empty;
            }
            _categoryRepository.InsertCategory(category);
            _categoryRepository.Save();

            return RedirectToAction("CategoriesList");
        }

        [Authorize(Roles = "Admin")]

        public IActionResult UpdateCategory(Guid id) 
        {
            var categories = _categoryRepository.GetById(id);
            return View(categories);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult UpdateCategory(Category category, IFormFile ImageUrl)
        {
            var cg = _categoryRepository.GetById(category.Id);

            //if (!string.IsNullOrEmpty(cg.ImageUrl) && ImageUrl != null && ImageUrl.Length > 0 )
            //{
            //    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cg.ImageUrl.TrimStart('/'));
            //    if (System.IO.File.Exists(oldImagePath))
            //    {
            //        System.IO.File.Delete(oldImagePath);
            //    }
            //}

            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img", ImageUrl.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }
                cg.ImageUrl = $"/assets/img/{ImageUrl.FileName}";
            }
            else
            {
                cg.ImageUrl = cg.ImageUrl;
            }

            _categoryRepository.UpdateCategory(cg);
            _categoryRepository.Save();

            return RedirectToAction("CategoriesList");

        }

        [Authorize(Roles = "Admin")]

        public IActionResult DeleteCategory(Guid id) 
        {
            var category = _categoryRepository.GetById(id);
            return View(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult DeleteCategory(Category category) 
        {
            var cg = _categoryRepository.GetById(category.Id);
            //if (!string.IsNullOrEmpty(cg.ImageUrl))
            //{
            //    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cg.ImageUrl.TrimStart('/'));
            //    if (System.IO.File.Exists(oldImagePath))
            //    {
            //        System.IO.File.Delete(oldImagePath);
            //    }
            //}
            _categoryRepository.DeleteCategory(cg);
            _categoryRepository.Save();
            return RedirectToAction("CategoriesList");
        }
        [Authorize(Roles = "Admin")]

        public IActionResult MenuItemsList()
        {
            return View(_menuItemRepository.GetAll());
        }
        [Authorize(Roles = "Admin")]

        public IActionResult CreateMenuItem()
        {
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult CreateMenuItem(MenuItem menuItem, IFormFile ImageUrl)
        {
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img", ImageUrl.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }
                menuItem.ImageUrl = $"/assets/img/{ImageUrl.FileName}";
            }
            if (menuItem.Description == null)
            {
                menuItem.Description = string.Empty;
            }
            _menuItemRepository.InsertMenuItem(menuItem);
            _menuItemRepository.Save();
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return RedirectToAction("MenuItemsList");
        }
        [Authorize(Roles = "Admin")]

        public IActionResult UpdateMenuItem(Guid id)
        {
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View(_menuItemRepository.GetById(id));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult UpdateMenuItem(MenuItem menuItem, IFormFile ImageUrl)
        {
            var mi = _menuItemRepository.GetById(menuItem.Id);
            //if (!string.IsNullOrEmpty(mi.ImageUrl))
            //{
            //    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", mi.ImageUrl.TrimStart('/'));
            //    if (System.IO.File.Exists(oldImagePath))
            //    {
            //        System.IO.File.Delete(oldImagePath);
            //    }
                
            //}
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img", ImageUrl.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }
                menuItem.ImageUrl = $"/assets/img/{ImageUrl.FileName}";
            }
            _menuItemRepository.UpdateMenuItem(menuItem);
            _menuItemRepository.Save();
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return RedirectToAction("MenuItemsList");
        }
        [Authorize(Roles = "Admin")]

        public IActionResult DeleteMenuItem(Guid id)
        {
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View(_menuItemRepository.GetById(id));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult DeleteMenuItem(MenuItem menuItem)
        {
            var mi = _menuItemRepository.GetById(menuItem.Id);
            //if (!string.IsNullOrEmpty(mi.ImageUrl))
            //{
            //    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", mi.ImageUrl.TrimStart('/'));
            //    if (System.IO.File.Exists(oldImagePath))
            //    {
            //        System.IO.File.Delete(oldImagePath);
            //    }
            //}
            _menuItemRepository.DeleteMenuItem(mi);
            _menuItemRepository.Save();
            var categories = _categoryRepository.GetAll();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return RedirectToAction("MenuItemsList");
        }
    }
}
