using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ticomarkenet.Data;
using ticomarkenet.Models;

namespace ticomarkenet.Controllers
{
    public class ImagenesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImagenesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Imagenes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Imagenes.Include(i => i.Producto);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Imagenes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var imagen = await _context.Imagenes
                .Include(i => i.Producto)
                .FirstOrDefaultAsync(m => m.ImagenId == id);
            if (imagen == null) return NotFound();

            return View(imagen);
        }

        // GET: Imagenes/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Nombre");
            return View();
        }

        // POST: Imagenes/Create (permite múltiples archivos)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ProductoId, List<IFormFile> archivos)
        {
            if (archivos != null && archivos.Any())
            {
                var carpetaImagenes = Path.Combine(_env.WebRootPath, "imagenes");

                if (!Directory.Exists(carpetaImagenes))
                {
                    Directory.CreateDirectory(carpetaImagenes);
                }

                foreach (var archivo in archivos)
                {
                    if (archivo.Length > 0)
                    {
                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
                        var rutaFisica = Path.Combine(carpetaImagenes, nombreArchivo);

                        using (var stream = new FileStream(rutaFisica, FileMode.Create))
                        {
                            await archivo.CopyToAsync(stream);
                        }

                        var imagen = new Imagen
                        {
                            Ruta = nombreArchivo,
                            ProductoId = ProductoId
                        };

                        _context.Add(imagen);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Debe seleccionar al menos una imagen válida.");
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Nombre", ProductoId);
            return View();
        }

        // GET: Imagenes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var imagen = await _context.Imagenes.FindAsync(id);
            if (imagen == null) return NotFound();

            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Nombre", imagen.ProductoId);
            return View(imagen);
        }

        // POST: Imagenes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImagenId,Ruta,ProductoId")] Imagen imagen)
        {
            if (id != imagen.ImagenId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagenExists(imagen.ImagenId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Nombre", imagen.ProductoId);
            return View(imagen);
        }

        // GET: Imagenes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var imagen = await _context.Imagenes
                .Include(i => i.Producto)
                .FirstOrDefaultAsync(m => m.ImagenId == id);
            if (imagen == null) return NotFound();

            return View(imagen);
        }

        // POST: Imagenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imagen = await _context.Imagenes.FindAsync(id);
            if (imagen != null)
            {
                var rutaFisica = Path.Combine(_env.WebRootPath, "imagenes", imagen.Ruta);
                if (System.IO.File.Exists(rutaFisica))
                {
                    System.IO.File.Delete(rutaFisica);
                }

                _context.Imagenes.Remove(imagen);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ImagenExists(int id)
        {
            return _context.Imagenes.Any(e => e.ImagenId == id);
        }
        public IActionResult IndexI()
        {
            return View();
        }
    }
}
