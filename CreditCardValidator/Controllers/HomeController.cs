using Application.Common.Interfaces;
using CreditCardValidator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CreditCardValidator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {

            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var creditCards = _unitOfWork.CreditCardRepository.GetAll();
            var creditCardProviders = _unitOfWork.CreditCardProviderRepository.GetAll();

            var ccpList = await creditCardProviders.ToListAsync();
            var ccList = await creditCards.ToListAsync();

            return View();
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