using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;
using System.Security.Claims;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class SettingsController : Controller
    {
        private readonly IAccountService _account;
        private readonly ICategoryService _category;
        private readonly IMapper _mapper;

        public SettingsController(IAccountService account, ICategoryService category, IMapper mapper)
        {
            _account = account;
            _category = category;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Accounts()
        {
            var settingsVM = new SettingsVM();
            settingsVM.Accounts = _account.GetAll();
            return View(settingsVM);
        }

        public async Task<IActionResult> EditAccount(int id)
        {
            var accountVM = new AccountVM();
            accountVM = _mapper.Map<AccountVM>(_account.GetById(id));
            return View(accountVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(int id, AccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                _account.Save(_mapper.Map<AccountModel>(accountVM));
                return RedirectToAction(nameof(Accounts));
            }

            return View(accountVM);
        }

        public async Task<IActionResult> DeleteAccount(int id)
        {
            return View(_mapper.Map<AccountVM>(_account.GetById(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(int id, AccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                _account.Delete(id);
                return RedirectToAction(nameof(Accounts));
            }

            return View(accountVM);
        }

        public IActionResult Categories()
        {
            var settingsVM = new SettingsVM();
            settingsVM.Categories = _category.GetAll();
            return View(settingsVM);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                _category.Save(_mapper.Map<CategoryModel>(categoryVM));
                return RedirectToAction(nameof(Categories));
            }

            return View(categoryVM);
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var categoryVM = new CategoryVM();

            categoryVM = _mapper.Map<CategoryVM>(_category.GetById(id));

            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                _category.Save(_mapper.Map<CategoryModel>(categoryVM));
                return RedirectToAction(nameof(Categories));
            }

            return View(categoryVM);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryVM = new CategoryVM();

            categoryVM = _mapper.Map<CategoryVM>(_category.GetById(id));

            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id, CategoryVM categoryVM)
        {
            _category.Delete(categoryVM.Id);
            return RedirectToAction(nameof(Categories));
        }
    }
}
