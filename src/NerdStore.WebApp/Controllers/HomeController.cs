using Microsoft.AspNetCore.Mvc;
using NerdStore.Pedido.Commands;
using NerdStore.WebApp.Models;
using Rebus.Bus;
using System.Diagnostics;

namespace NerdStore.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBus _bus;

        public HomeController(IBus bus)
        {
            _bus = bus;
        }

        public IActionResult Index()
        {
            _bus.Send(new RealizarPedidoCommand { AggregateRoot = Guid.NewGuid() }).Wait();

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
