using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> prodList = _unitOfWork.Product.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(prodList);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product prod) 
        {
            if(ModelState.IsValid) { 
                _unitOfWork.Product.Add(prod);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfuly";
                return RedirectToAction("Index","Product");
            }
            return View();
        }
        public IActionResult Edit(int id) 
        {
            if (id == null || id == 0) return NotFound();
            Product? productFromDb = _unitOfWork.Product.Get(p => p.Id == id);
            if(productFromDb == null) return NotFound();
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product product) 
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Model error: {error.ErrorMessage}");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product edited successfuly";
                return RedirectToAction("Index", "Product");
            }
           
            return View();
        }
        public IActionResult Delete(int? id) 
        {
            if (id == null || id == 0) return NotFound();
            Product? productFromDb = _unitOfWork.Product.Get(p => p.Id == id);
            if (productFromDb == null) return NotFound();
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? productFromDb = _unitOfWork.Product.Get(p => p.Id == id);
            if (productFromDb == null) return NotFound();
            _unitOfWork.Product.Remove(productFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfuly";
            return RedirectToAction("Index", "Product");
        }
    }
}
