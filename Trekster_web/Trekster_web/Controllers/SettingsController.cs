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
        private readonly UserManager<User> _userManager;
        private readonly IAccountService _account;
        private readonly ICategoryService _category;

        public SettingsController(IAccountService account, UserManager<User> userManager, ICategoryService category)
        {
            _account = account;
            _userManager = userManager;
            _category = category;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Accounts()
        {
            var settingsVM = new SettingsVM();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            settingsVM.Accounts = _account.GetAll().Where(x => x.User.Id == userId);
            return View(settingsVM);
        }

        public async Task<IActionResult> EditAccount(int id)
        {
            var settingsVM = new SettingsVM();
            settingsVM.Account = _account.GetById(id);
            return View(settingsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(int id, SettingsVM settingsVM)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            settingsVM.Account.User = user;

            _account.Save(settingsVM.Account);
            return RedirectToAction(nameof(Accounts));
        }

        public async Task<IActionResult> DeleteAccount(int id)
        {
            var settingsVM = new SettingsVM();
            settingsVM.Account = _account.GetById(id);
            return View(settingsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(int id, SettingsVM settingsVM)
        {
            _account.Delete(id);
            return RedirectToAction(nameof(Accounts));
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
        public async Task<IActionResult> Create(SettingsVM settingsVM)
        {
            _category.Save(settingsVM.Category);
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var settingsVM = new SettingsVM();
            settingsVM.Category = _category.GetById(id);
            return View(settingsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, SettingsVM settingsVM)
        {
            _category.Save(settingsVM.Category);
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var settingsVM = new SettingsVM();
            settingsVM.Category = _category.GetById(id);
            return View(settingsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id, SettingsVM settingsVM)
        {
            _category.Delete(id);
            return RedirectToAction(nameof(Categories));
        }
    }
}
