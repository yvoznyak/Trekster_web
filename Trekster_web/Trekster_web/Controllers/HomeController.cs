using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trekster_web.ControllerServices.Interfaces;
using Trekster_web.Filters;
using Trekster_web.Models;

namespace Trekster_web.Controllers
{
    [AuthorizeFilter]
    public class HomeController : Controller
    {
        private readonly ICategoryService _category;
        private readonly IHomeControllerService _home;

        public HomeController(ICategoryService category,
                              IHomeControllerService home)
        {
            _category = category;
            _home = home;
        }

        public IActionResult Index()
        {
            var homeVM = new HomeVM();
            homeVM.Summary = _home.GetSummary();
            homeVM.ExpencesPercentage = _home.GetExpencesPercentage();
            homeVM.ProfitsPercentage = 100 - homeVM.ExpencesPercentage;
            homeVM.ButtonExist = _home.ButtonExist();
            return View(homeVM);
        }

        public IActionResult CreateTransaction()
        {
            ViewData["AccountsAndCurency"] = _home.GetListOfAccounts();
            ViewData["Categories"] = new SelectList(_category.GetAll(), "Id", "Name");
            var transactionVM = new TransactionVM();
            return View(transactionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransaction(TransactionVM transactionVM)
        {
            if (ModelState.IsValid)
            {
                _home.SaveTransaction(transactionVM);
                return RedirectToAction(nameof(Index));
            }

            return View(transactionVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}