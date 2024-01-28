using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList =_categoryRepository.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //if(obj.Name == obj.DisplayOrder.ToString()) 
            //{
            //    ModelState.AddModelError("name", "Display Order cannot match Name");
            //}
            if(ModelState.IsValid)
            {
                _categoryRepository.Add(obj);
                _categoryRepository.Save();
                TempData["success"] = "Category created successfuly!";
                return RedirectToAction("Index","Category");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Category? categoryFromDb = _categoryRepository.Get(u => u.Id==id);
            if(categoryFromDb == null) return NotFound();
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            //if(obj.Name == obj.DisplayOrder.ToString()) 
            //{
            //    ModelState.AddModelError("name", "Display Order cannot match Name");
            //}
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(obj);
                _categoryRepository.Save();
                TempData["success"] = "Category updated successfuly!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Category? categoryFromDb = _categoryRepository.Get(u => u.Id == id);
            if (categoryFromDb == null) return NotFound();
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category obj = _categoryRepository.Get(u => u.Id == id);
            if(obj == null) return NotFound();
            _categoryRepository.Remove(obj);
            _categoryRepository.Save();
            TempData["success"] = "Category deleted successfuly!";
            return RedirectToAction("Index", "Category");
        }
            
        
    }
}

