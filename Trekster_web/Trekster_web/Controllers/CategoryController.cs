using AutoMapper;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService category)
        {
            _categoryService = category;
        }

        public IActionResult Index()
        {
            var categoryVM = new CategoryVM();
            categoryVM.CategoryModels = _categoryService.GetAll();
            return View(categoryVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryModel")] CategoryVM categoryVM)
        {
            _categoryService.Save(categoryVM.CategoryModel);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoryVM = new CategoryVM();
            categoryVM.CategoryModel = _categoryService.GetById(id);
            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryModel")] CategoryVM categoryVM)
        {
            _categoryService.Save(categoryVM.CategoryModel);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var categoryVM = new CategoryVM();
            categoryVM.CategoryModel = _categoryService.GetById(id);
            return View(categoryVM);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _categoryService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
