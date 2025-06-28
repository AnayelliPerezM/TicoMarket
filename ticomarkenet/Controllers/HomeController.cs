using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ticomarkenet.Data; // Asegurate de importar la carpeta correcta
using ticomarkenet.Models;

namespace ticomarkenet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context; // Usa tu clase de contexto

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Cargar productos con sus imágenes
            var productos = _context.Productos
                                    .Include(p => p.Imagenes)
                                    .ToList();


            if (!productos.Any())
            {
                // Esto es solo para verificar
                Console.WriteLine("No hay productos en la base de datos.");
            }
            return View(productos); // Pasa la lista a la vista Index.cshtml
        }

        public IActionResult Nosotros()
        {
            return View();
        }

        public IActionResult Contacto()
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
