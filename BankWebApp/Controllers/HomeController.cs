using BankWebApp.Models;
using BankWebApp.Services;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace BankWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly OrderManager _orderWatcher;

        public HomeController(OrderManager orderWatcher)
        {
            _orderWatcher = orderWatcher;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _orderWatcher.GetAll());
        }
        
        public async Task<IActionResult> GetFirst()
        {
            return View(await _orderWatcher.Peek());
        }
        
        public async Task RemoveFirst()
        {
            await _orderWatcher.DequeueMessage();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}